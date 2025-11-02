using Microsoft.EntityFrameworkCore;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;
using MedicalRecordManagement.Infrastructure.Data;

namespace MedicalRecordManagement.Infrastructure.Repositories
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(MedicalRecordDbContext context) : base(context)
        {
        }

        public async Task<MedicalRecord> GetByRecordNumberAsync(string recordNumber)
        {
            return await _dbSet
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .FirstOrDefaultAsync(mr => mr.RecordNumber == recordNumber);
        }

        public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _dbSet
                .Include(mr => mr.Doctor)
                .Where(mr => mr.PatientId == patientId)
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(int doctorId)
        {
            return await _dbSet
                .Include(mr => mr.Patient)
                .Where(mr => mr.DoctorId == doctorId)
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .Where(mr => mr.VisitDate >= startDate && mr.VisitDate <= endDate)
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<MedicalRecord> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .Include(mr => mr.MedicalTests)
                .Include(mr => mr.Prescriptions)
                .FirstOrDefaultAsync(mr => mr.Id == id);
        }
    }
}
