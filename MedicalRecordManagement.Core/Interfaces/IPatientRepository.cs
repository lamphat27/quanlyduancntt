using MedicalRecordManagement.Core.Entities;

namespace MedicalRecordManagement.Core.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetByPatientCodeAsync(string patientCode);
        Task<Patient> GetByNationalIdAsync(string nationalId);
        Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm);
        Task<IEnumerable<Patient>> GetPatientsByDoctorAsync(int doctorId);
    }
}
