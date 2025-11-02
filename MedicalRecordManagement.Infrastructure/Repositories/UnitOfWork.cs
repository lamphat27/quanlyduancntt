using Microsoft.EntityFrameworkCore.Storage;
using MedicalRecordManagement.Core.Interfaces;
using MedicalRecordManagement.Infrastructure.Data;

namespace MedicalRecordManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MedicalRecordDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(MedicalRecordDbContext context)
        {
            _context = context;
            Patients = new PatientRepository(_context);
            Doctors = new DoctorRepository(_context);
            MedicalRecords = new MedicalRecordRepository(_context);
            Appointments = new AppointmentRepository(_context);
            MedicalTests = new GenericRepository<Core.Entities.MedicalTest>(_context);
            Prescriptions = new GenericRepository<Core.Entities.Prescription>(_context);
            Users = new GenericRepository<Core.Entities.User>(_context);
        }

        public IPatientRepository Patients { get; private set; }
        public IDoctorRepository Doctors { get; private set; }
        public IMedicalRecordRepository MedicalRecords { get; private set; }
        public IAppointmentRepository Appointments { get; private set; }
        public IGenericRepository<Core.Entities.MedicalTest> MedicalTests { get; private set; }
        public IGenericRepository<Core.Entities.Prescription> Prescriptions { get; private set; }
        public IGenericRepository<Core.Entities.User> Users { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
