using Microsoft.EntityFrameworkCore;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;
using MedicalRecordManagement.Infrastructure.Data;

namespace MedicalRecordManagement.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MedicalRecordDbContext context) : base(context)
        {
        }

        public async Task<Doctor> GetByDoctorCodeAsync(string doctorCode)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.DoctorCode == doctorCode);
        }

        public async Task<Doctor> GetByLicenseNumberAsync(string licenseNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);
        }

        public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization)
        {
            return await _dbSet
                .Where(d => d.Specialization.Contains(specialization))
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
        {
            return await _dbSet
                .Where(d => d.IsActive)
                .ToListAsync();
        }
    }
}
