using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
namespace HospitalManagementSystem.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;
        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("Password", "Email and Password are required.");
                return View();
            }

            var existingUser = await _context.Patients.FirstOrDefaultAsync(u => u.Email == Email);
            
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(Password, existingUser.Password))
            {
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View();
            }

            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, existingUser.Email),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, existingUser.FullName)
            };

            var identity = new System.Security.Claims.ClaimsIdentity(claims, "CookieAuth");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Dashboard");
        }       

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("Email", "Email is required.");
                return View();
            }

            var user = await _context.Patients.FirstOrDefaultAsync(u => u.Email == Email);
            
            // SECURITY BEST PRACTICE: Always display the exact same message whether the email exists or not.
            // This prevents "Email Enumeration" attacks where hackers guess emails to see who is registered.
            ViewBag.Message = "If an account with that email exists, a password reset link has been sent.";
            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(Patient patient)
        {
            if (ModelState.IsValid)
            {
                bool emailExists = await _context.Patients.AnyAsync(p => p.Email == patient.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(patient);
                }

                patient.Password = BCrypt.Net.BCrypt.HashPassword(patient.Password);

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(patient);
        }

        public async Task<IActionResult> Dashboard()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdStr, out int userId))
            {
                var user = await _context.Patients.FindAsync(userId);
                if (user != null)
                {
                    ViewBag.FullName = user.FullName;
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }


    }
}
