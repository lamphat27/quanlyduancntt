using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordManagement.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "User"; // Admin, Doctor, Nurse, Receptionist, User

        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginDate { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        // Navigation properties
        public int? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        // Computed property
        public string FullName => $"{FirstName} {LastName}";
    }
}
