using MedicalRecordManagement.Core.Entities;

namespace MedicalRecordManagement.Core.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor> GetByDoctorCodeAsync(string doctorCode);
        Task<Doctor> GetByLicenseNumberAsync(string licenseNumber);
        Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization);
        Task<IEnumerable<Doctor>> GetActiveDoctorsAsync();
    }
}
