using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalManagementSystem.Controllers;

public class DoctorController : Controller
{
    private readonly ApplicationDbContext _context;

    public DoctorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        // If already logged in, redirect to dashboard
        if (User.Identity != null && User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Role" && c.Value == "Doctor"))
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Doctor model, bool remember)
    {
        // Simple login logic
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == model.Email);
        
        // Use basic check for prototype. In production, use BCrypt to verify hashed password.
        if (doctor != null && doctor.Password == model.Password)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, doctor.Email),
                new Claim("FullName", doctor.FullName),
                new Claim("Role", "Doctor"),
                new Claim("DoctorId", doctor.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = remember,
                ExpiresUtc = remember ? DateTimeOffset.UtcNow.AddDays(30) : null
            };

            await HttpContext.SignInAsync("CookieAuth", principal, authProperties);

            return RedirectToAction("Dashboard");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Signup()
    {
        ViewBag.Departments = await _context.Departments.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signup([Bind("FullName,Email,Password,Phone,Specialization,DepartmentId,Qualifications")] Doctor model)
    {
        ModelState.Remove("Department");
        
        if (ModelState.IsValid)
        {
            // Simple signup, no hash for prototype
            _context.Doctors.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        
        ViewBag.Departments = await _context.Departments.ToListAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        // Require doctor role
        if (User.Identity == null || !User.Identity.IsAuthenticated || !User.HasClaim(c => c.Type == "Role" && c.Value == "Doctor"))
        {
            return RedirectToAction("Login");
        }

        // Fetch name
        var doctorIdStr = User.FindFirst("DoctorId")?.Value;
        if (int.TryParse(doctorIdStr, out int doctorId))
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor != null)
            {
                ViewBag.FullName = doctor.FullName;
            }
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated || !User.HasClaim(c => c.Type == "Role" && c.Value == "Doctor"))
        {
            return RedirectToAction("Login");
        }

        var doctorIdStr = User.FindFirst("DoctorId")?.Value;
        if (int.TryParse(doctorIdStr, out int doctorId))
        {
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == doctorId);
                
            if (doctor != null)
            {
                ViewBag.FullName = doctor.FullName;
                return View(doctor);
            }
        }
        
        return RedirectToAction("Dashboard");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Index", "Home");
    }
}
