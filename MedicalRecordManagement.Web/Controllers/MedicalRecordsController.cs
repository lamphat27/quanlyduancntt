using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;

namespace MedicalRecordManagement.Web.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicalRecordsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: MedicalRecords
        public async Task<IActionResult> Index()
        {
            var medicalRecords = await _unitOfWork.MedicalRecords.GetAllAsync();
            return View(medicalRecords);
        }

        // GET: MedicalRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _unitOfWork.MedicalRecords.GetWithDetailsAsync(id.Value);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // GET: MedicalRecords/Create
        public async Task<IActionResult> Create()
        {
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName");
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName");
            return View();
        }

        // POST: MedicalRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,DoctorId,ChiefComplaint,PresentIllness,PhysicalExamination,Diagnosis,Treatment,Prescription,Notes,ConsultationFee")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                // Generate record number
                var count = await _unitOfWork.MedicalRecords.CountAsync();
                medicalRecord.RecordNumber = $"MR{(count + 1):D6}";
                medicalRecord.VisitDate = DateTime.Now;
                medicalRecord.Status = "Active";

                await _unitOfWork.MedicalRecords.AddAsync(medicalRecord);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName", medicalRecord.PatientId);
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName", medicalRecord.DoctorId);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _unitOfWork.MedicalRecords.GetByIdAsync(id.Value);
            if (medicalRecord == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName", medicalRecord.PatientId);
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName", medicalRecord.DoctorId);
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecordNumber,PatientId,DoctorId,VisitDate,ChiefComplaint,PresentIllness,PhysicalExamination,Diagnosis,Treatment,Prescription,Notes,ConsultationFee,Status")] MedicalRecord medicalRecord)
        {
            if (id != medicalRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.MedicalRecords.UpdateAsync(medicalRecord);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!await MedicalRecordExists(medicalRecord.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName", medicalRecord.PatientId);
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName", medicalRecord.DoctorId);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _unitOfWork.MedicalRecords.GetWithDetailsAsync(id.Value);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecord = await _unitOfWork.MedicalRecords.GetByIdAsync(id);
            if (medicalRecord != null)
            {
                await _unitOfWork.MedicalRecords.DeleteAsync(medicalRecord);
                await _unitOfWork.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MedicalRecordExists(int id)
        {
            return await _unitOfWork.MedicalRecords.ExistsAsync(mr => mr.Id == id);
        }
    }
}
