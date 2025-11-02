using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordManagement.Core.Entities
{
    public class Doctor : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string DoctorCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; }

        [StringLength(20)]
        public string LicenseNumber { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public DateTime? LicenseExpiryDate { get; set; }

        [StringLength(500)]
        public string Qualifications { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}";
    }
}
