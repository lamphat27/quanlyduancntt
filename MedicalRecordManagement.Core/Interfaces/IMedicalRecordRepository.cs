using MedicalRecordManagement.Core.Entities;

namespace MedicalRecordManagement.Core.Interfaces
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        Task<MedicalRecord> GetByRecordNumberAsync(string recordNumber);
        Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<MedicalRecord> GetWithDetailsAsync(int id);
    }
}
