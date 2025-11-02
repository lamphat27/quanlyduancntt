using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordManagement.Core.Entities
{
    public class Prescription : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string MedicationName { get; set; }

        public int MedicalRecordId { get; set; }

        [StringLength(200)]
        public string Dosage { get; set; }

        [StringLength(100)]
        public string Frequency { get; set; }

        [StringLength(100)]
        public string Duration { get; set; }

        [StringLength(500)]
        public string Instructions { get; set; }

        public int? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Completed, Cancelled

        // Navigation properties
        public virtual MedicalRecord MedicalRecord { get; set; }
    }
}
