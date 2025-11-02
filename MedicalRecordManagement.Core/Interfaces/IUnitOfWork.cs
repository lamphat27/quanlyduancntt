using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;

namespace MedicalRecordManagement.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        IDoctorRepository Doctors { get; }
        IMedicalRecordRepository MedicalRecords { get; }
        IAppointmentRepository Appointments { get; }
        IGenericRepository<MedicalTest> MedicalTests { get; }
        IGenericRepository<Prescription> Prescriptions { get; }
        IGenericRepository<User> Users { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
