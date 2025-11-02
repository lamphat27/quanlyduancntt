using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordManagement.Core.Entities
{
    public class Appointment : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string AppointmentNumber { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Confirmed, Completed, Cancelled, NoShow

        [StringLength(1000)]
        public string Notes { get; set; }

        public decimal? ConsultationFee { get; set; }

        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
