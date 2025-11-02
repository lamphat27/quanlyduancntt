using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Infrastructure.Data;

namespace MedicalRecordManagement.Infrastructure.SeedData
{
    public static class DbInitializer
    {
        public static async Task Initialize(MedicalRecordDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (context.Patients.Any())
            {
                return; // Database has been seeded
            }

            // Seed Doctors
            var doctors = new Doctor[]
            {
                new Doctor
                {
                    FirstName = "Nguyễn Văn",
                    LastName = "An",
                    DoctorCode = "D000001",
                    Specialization = "Tim mạch",
                    LicenseNumber = "BS001",
                    PhoneNumber = "0123456789",
                    Email = "nguyenvanan@hospital.com",
                    Address = "123 Đường ABC, Quận 1, TP.HCM",
                    LicenseExpiryDate = DateTime.Now.AddYears(5),
                    Qualifications = "Tiến sĩ Y khoa, Chuyên khoa Tim mạch",
                    IsActive = true
                },
                new Doctor
                {
                    FirstName = "Trần Thị",
                    LastName = "Bình",
                    DoctorCode = "D000002",
                    Specialization = "Nhi khoa",
                    LicenseNumber = "BS002",
                    PhoneNumber = "0987654321",
                    Email = "tranthibinh@hospital.com",
                    Address = "456 Đường XYZ, Quận 2, TP.HCM",
                    LicenseExpiryDate = DateTime.Now.AddYears(3),
                    Qualifications = "Thạc sĩ Y khoa, Chuyên khoa Nhi",
                    IsActive = true
                },
                new Doctor
                {
                    FirstName = "Lê Văn",
                    LastName = "Cường",
                    DoctorCode = "D000003",
                    Specialization = "Ngoại khoa",
                    LicenseNumber = "BS003",
                    PhoneNumber = "0369852147",
                    Email = "levancuong@hospital.com",
                    Address = "789 Đường DEF, Quận 3, TP.HCM",
                    LicenseExpiryDate = DateTime.Now.AddYears(4),
                    Qualifications = "Tiến sĩ Y khoa, Chuyên khoa Ngoại",
                    IsActive = true
                }
            };

            context.Doctors.AddRange(doctors);
            await context.SaveChangesAsync();

            // Seed Patients
            var patients = new Patient[]
            {
                new Patient
                {
                    FirstName = "Phạm Văn",
                    LastName = "Đức",
                    PatientCode = "P000001",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Gender = "Nam",
                    Address = "101 Đường GHI, Quận 4, TP.HCM",
                    PhoneNumber = "0123456780",
                    Email = "phamvanduc@email.com",
                    NationalId = "123456789",
                    MedicalHistory = "Tiền sử cao huyết áp",
                    Allergies = "Không có"
                },
                new Patient
                {
                    FirstName = "Nguyễn Thị",
                    LastName = "Hoa",
                    PatientCode = "P000002",
                    DateOfBirth = new DateTime(1990, 8, 22),
                    Gender = "Nữ",
                    Address = "202 Đường JKL, Quận 5, TP.HCM",
                    PhoneNumber = "0987654320",
                    Email = "nguyenthihoa@email.com",
                    NationalId = "987654321",
                    MedicalHistory = "Tiền sử tiểu đường",
                    Allergies = "Dị ứng penicillin"
                },
                new Patient
                {
                    FirstName = "Trần Văn",
                    LastName = "Minh",
                    PatientCode = "P000003",
                    DateOfBirth = new DateTime(1978, 12, 10),
                    Gender = "Nam",
                    Address = "303 Đường MNO, Quận 6, TP.HCM",
                    PhoneNumber = "0369852140",
                    Email = "tranvanminh@email.com",
                    NationalId = "456789123",
                    MedicalHistory = "Không có",
                    Allergies = "Không có"
                }
            };

            context.Patients.AddRange(patients);
            await context.SaveChangesAsync();

            // Seed Appointments
            var appointments = new Appointment[]
            {
                new Appointment
                {
                    AppointmentNumber = "APT000001",
                    PatientId = patients[0].Id,
                    DoctorId = doctors[0].Id,
                    AppointmentDate = DateTime.Now.AddDays(1),
                    AppointmentTime = new TimeSpan(9, 0, 0),
                    Reason = "Khám định kỳ tim mạch",
                    Status = "Scheduled",
                    Notes = "Bệnh nhân cần nhịn ăn trước khi khám",
                    ConsultationFee = 200000
                },
                new Appointment
                {
                    AppointmentNumber = "APT000002",
                    PatientId = patients[1].Id,
                    DoctorId = doctors[1].Id,
                    AppointmentDate = DateTime.Now.AddDays(2),
                    AppointmentTime = new TimeSpan(14, 30, 0),
                    Reason = "Khám sức khỏe tổng quát",
                    Status = "Scheduled",
                    Notes = "Kiểm tra đường huyết",
                    ConsultationFee = 150000
                }
            };

            context.Appointments.AddRange(appointments);
            await context.SaveChangesAsync();

            // Seed Medical Records
            var medicalRecords = new MedicalRecord[]
            {
                new MedicalRecord
                {
                    RecordNumber = "MR000001",
                    PatientId = patients[0].Id,
                    DoctorId = doctors[0].Id,
                    VisitDate = DateTime.Now.AddDays(-5),
                    ChiefComplaint = "Đau ngực, khó thở",
                    PresentIllness = "Bệnh nhân xuất hiện đau ngực từ 3 ngày trước, kèm theo khó thở khi vận động",
                    PhysicalExamination = "Huyết áp: 140/90 mmHg, Nhịp tim: 85 lần/phút, Không có tiếng thổi tim",
                    Diagnosis = "Cao huyết áp, Rối loạn nhịp tim nhẹ",
                    Treatment = "Điều chỉnh chế độ ăn, tập thể dục nhẹ, theo dõi huyết áp",
                    Prescription = "Amlodipine 5mg x 1 viên/ngày",
                    Notes = "Tái khám sau 2 tuần",
                    ConsultationFee = 200000,
                    Status = "Active"
                },
                new MedicalRecord
                {
                    RecordNumber = "MR000002",
                    PatientId = patients[1].Id,
                    DoctorId = doctors[1].Id,
                    VisitDate = DateTime.Now.AddDays(-3),
                    ChiefComplaint = "Mệt mỏi, khát nước nhiều",
                    PresentIllness = "Bệnh nhân cảm thấy mệt mỏi và khát nước liên tục trong 1 tuần qua",
                    PhysicalExamination = "Đường huyết: 180 mg/dL, Cân nặng: 65kg, Chiều cao: 160cm",
                    Diagnosis = "Tiểu đường type 2",
                    Treatment = "Chế độ ăn kiêng, tập thể dục, kiểm soát đường huyết",
                    Prescription = "Metformin 500mg x 2 viên/ngày",
                    Notes = "Hướng dẫn chế độ ăn và tập luyện",
                    ConsultationFee = 150000,
                    Status = "Active"
                }
            };

            context.MedicalRecords.AddRange(medicalRecords);
            await context.SaveChangesAsync();

            // Seed Medical Tests
            var medicalTests = new MedicalTest[]
            {
                new MedicalTest
                {
                    TestName = "Xét nghiệm máu tổng quát",
                    MedicalRecordId = medicalRecords[0].Id,
                    TestDate = DateTime.Now.AddDays(-4),
                    TestResults = "HbA1c: 6.2%, Cholesterol: 220 mg/dL, Triglyceride: 180 mg/dL",
                    NormalRange = "HbA1c < 5.7%, Cholesterol < 200 mg/dL, Triglyceride < 150 mg/dL",
                    Status = "Completed",
                    Notes = "Kết quả cho thấy có dấu hiệu tiền tiểu đường",
                    TestCost = 150000
                },
                new MedicalTest
                {
                    TestName = "Điện tâm đồ",
                    MedicalRecordId = medicalRecords[0].Id,
                    TestDate = DateTime.Now.AddDays(-4),
                    TestResults = "Nhịp xoang đều, 85 lần/phút, Không có dấu hiệu bất thường",
                    NormalRange = "Nhịp xoang 60-100 lần/phút",
                    Status = "Completed",
                    Notes = "Kết quả bình thường",
                    TestCost = 100000
                }
            };

            context.MedicalTests.AddRange(medicalTests);
            await context.SaveChangesAsync();

            // Seed Prescriptions
            var prescriptions = new Prescription[]
            {
                new Prescription
                {
                    MedicationName = "Amlodipine",
                    MedicalRecordId = medicalRecords[0].Id,
                    Dosage = "5mg",
                    Frequency = "1 lần/ngày",
                    Duration = "30 ngày",
                    Instructions = "Uống sau bữa ăn sáng",
                    Quantity = 30,
                    UnitPrice = 5000,
                    Status = "Active"
                },
                new Prescription
                {
                    MedicationName = "Metformin",
                    MedicalRecordId = medicalRecords[1].Id,
                    Dosage = "500mg",
                    Frequency = "2 lần/ngày",
                    Duration = "30 ngày",
                    Instructions = "Uống sau bữa ăn sáng và tối",
                    Quantity = 60,
                    UnitPrice = 3000,
                    Status = "Active"
                }
            };

            context.Prescriptions.AddRange(prescriptions);
            await context.SaveChangesAsync();
        }
    }
}
