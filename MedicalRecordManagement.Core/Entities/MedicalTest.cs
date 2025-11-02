using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordManagement.Core.Entities
{
    public class MedicalTest : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TestName { get; set; }

        public int MedicalRecordId { get; set; }

        public DateTime TestDate { get; set; }

        [StringLength(1000)]
        public string TestResults { get; set; }

        [StringLength(500)]
        public string NormalRange { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Abnormal

        [StringLength(1000)]
        public string Notes { get; set; }

        public decimal? TestCost { get; set; }

        // Navigation properties
        public virtual MedicalRecord MedicalRecord { get; set; }
    }
}
