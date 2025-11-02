using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;

namespace MedicalRecordManagement.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            return View(appointments);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName");
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,DoctorId,AppointmentDate,AppointmentTime,Reason,Notes,ConsultationFee")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Generate appointment number
                var count = await _unitOfWork.Appointments.CountAsync();
                appointment.AppointmentNumber = $"APT{(count + 1):D6}";
                appointment.Status = "Scheduled";

                await _unitOfWork.Appointments.AddAsync(appointment);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName", appointment.PatientId);
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName", appointment.DoctorId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName", appointment.PatientId);
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName", appointment.DoctorId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppointmentNumber,PatientId,DoctorId,AppointmentDate,AppointmentTime,Reason,Status,Notes,ConsultationFee")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Appointments.UpdateAsync(appointment);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!await AppointmentExists(appointment.Id))
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
            ViewData["PatientId"] = new SelectList(await _unitOfWork.Patients.GetAllAsync(), "Id", "FullName", appointment.PatientId);
            ViewData["DoctorId"] = new SelectList(await _unitOfWork.Doctors.GetActiveDoctorsAsync(), "Id", "FullName", appointment.DoctorId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment != null)
            {
                await _unitOfWork.Appointments.DeleteAsync(appointment);
                await _unitOfWork.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AppointmentExists(int id)
        {
            return await _unitOfWork.Appointments.ExistsAsync(a => a.Id == id);
        }
    }
}
