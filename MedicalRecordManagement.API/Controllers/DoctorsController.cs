using Microsoft.AspNetCore.Mvc;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;

namespace MedicalRecordManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            return Ok(doctors);
        }

        // GET: api/doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // GET: api/doctors/code/D001
        [HttpGet("code/{doctorCode}")]
        public async Task<ActionResult<Doctor>> GetDoctorByCode(string doctorCode)
        {
            var doctor = await _unitOfWork.Doctors.GetByDoctorCodeAsync(doctorCode);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // GET: api/doctors/specialization/cardiology
        [HttpGet("specialization/{specialization}")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpecialization(string specialization)
        {
            var doctors = await _unitOfWork.Doctors.GetBySpecializationAsync(specialization);
            return Ok(doctors);
        }

        // GET: api/doctors/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetActiveDoctors()
        {
            var doctors = await _unitOfWork.Doctors.GetActiveDoctorsAsync();
            return Ok(doctors);
        }

        // POST: api/doctors
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate doctor code if not provided
            if (string.IsNullOrEmpty(doctor.DoctorCode))
            {
                doctor.DoctorCode = await GenerateDoctorCode();
            }

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        // PUT: api/doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _unitOfWork.Doctors.UpdateAsync(doctor);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!await DoctorExists(id))
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

        // DELETE: api/doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            await _unitOfWork.Doctors.DeleteAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> DoctorExists(int id)
        {
            return await _unitOfWork.Doctors.ExistsAsync(d => d.Id == id);
        }

        private async Task<string> GenerateDoctorCode()
        {
            var count = await _unitOfWork.Doctors.CountAsync();
            return $"D{(count + 1):D6}";
        }
    }
}
