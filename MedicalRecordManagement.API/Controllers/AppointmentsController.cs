using Microsoft.AspNetCore.Mvc;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;

namespace MedicalRecordManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            return Ok(appointments);
        }

        // GET: api/appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // GET: api/appointments/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByPatient(int patientId)
        {
            var appointments = await _unitOfWork.Appointments.GetByPatientIdAsync(patientId);
            return Ok(appointments);
        }

        // GET: api/appointments/doctor/5
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByDoctor(int doctorId)
        {
            var appointments = await _unitOfWork.Appointments.GetByDoctorIdAsync(doctorId);
            return Ok(appointments);
        }

        // GET: api/appointments/date/2024-01-15
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByDate(DateTime date)
        {
            var appointments = await _unitOfWork.Appointments.GetByDateAsync(date);
            return Ok(appointments);
        }

        // GET: api/appointments/upcoming?days=7
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetUpcomingAppointments([FromQuery] int days = 7)
        {
            var appointments = await _unitOfWork.Appointments.GetUpcomingAppointmentsAsync(days);
            return Ok(appointments);
        }

        // GET: api/appointments/number/APT001
        [HttpGet("number/{appointmentNumber}")]
        public async Task<ActionResult<Appointment>> GetAppointmentByNumber(string appointmentNumber)
        {
            var appointment = await _unitOfWork.Appointments.GetByAppointmentNumberAsync(appointmentNumber);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate appointment number if not provided
            if (string.IsNullOrEmpty(appointment.AppointmentNumber))
            {
                appointment.AppointmentNumber = await GenerateAppointmentNumber();
            }

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // PUT: api/appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _unitOfWork.Appointments.UpdateAsync(appointment);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!await AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            await _unitOfWork.Appointments.DeleteAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AppointmentExists(int id)
        {
            return await _unitOfWork.Appointments.ExistsAsync(a => a.Id == id);
        }

        private async Task<string> GenerateAppointmentNumber()
        {
            var count = await _unitOfWork.Appointments.CountAsync();
            return $"APT{(count + 1):D6}";
        }
    }
}
