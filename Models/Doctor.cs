using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models;

public class Doctor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;
    
    public string Qualifications { get; set; } = string.Empty;
    
    [Required]
    public int DepartmentId { get; set; }
    
    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
