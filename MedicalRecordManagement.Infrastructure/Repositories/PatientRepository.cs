using Microsoft.EntityFrameworkCore;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;
using MedicalRecordManagement.Infrastructure.Data;

namespace MedicalRecordManagement.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(MedicalRecordDbContext context) : base(context)
        {
        }

        public async Task<Patient> GetByPatientCodeAsync(string patientCode)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.PatientCode == patientCode);
        }

        public async Task<Patient> GetByNationalIdAsync(string nationalId)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.NationalId == nationalId);
        }

        public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(p => p.FirstName.Contains(searchTerm) || 
                           p.LastName.Contains(searchTerm) ||
                           p.PatientCode.Contains(searchTerm) ||
                           p.PhoneNumber.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsByDoctorAsync(int doctorId)
        {
            return await _dbSet
                .Where(p => p.MedicalRecords.Any(mr => mr.DoctorId == doctorId))
                .ToListAsync();
        }
    }
}
