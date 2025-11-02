using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordManagement.Core.Entities
{
    public class MedicalRecord : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string RecordNumber { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime VisitDate { get; set; }

        [StringLength(1000)]
        public string ChiefComplaint { get; set; }

        [StringLength(2000)]
        public string PresentIllness { get; set; }

        [StringLength(2000)]
        public string PhysicalExamination { get; set; }

        [StringLength(1000)]
        public string Diagnosis { get; set; }

        [StringLength(2000)]
        public string Treatment { get; set; }

        [StringLength(1000)]
        public string Prescription { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public decimal? ConsultationFee { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Completed, Cancelled

        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<MedicalTest> MedicalTests { get; set; } = new List<MedicalTest>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
