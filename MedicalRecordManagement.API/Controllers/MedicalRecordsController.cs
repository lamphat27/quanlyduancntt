using Microsoft.AspNetCore.Mvc;
using MedicalRecordManagement.Core.Entities;
using MedicalRecordManagement.Core.Interfaces;

namespace MedicalRecordManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicalRecordsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/medicalrecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecords()
        {
            var medicalRecords = await _unitOfWork.MedicalRecords.GetAllAsync();
            return Ok(medicalRecords);
        }

        // GET: api/medicalrecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int id)
        {
            var medicalRecord = await _unitOfWork.MedicalRecords.GetWithDetailsAsync(id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return Ok(medicalRecord);
        }

        // GET: api/medicalrecords/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecordsByPatient(int patientId)
        {
            var medicalRecords = await _unitOfWork.MedicalRecords.GetByPatientIdAsync(patientId);
            return Ok(medicalRecords);
        }

        // GET: api/medicalrecords/doctor/5
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecordsByDoctor(int doctorId)
        {
            var medicalRecords = await _unitOfWork.MedicalRecords.GetByDoctorIdAsync(doctorId);
            return Ok(medicalRecords);
        }

        // GET: api/medicalrecords/date-range?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecordsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var medicalRecords = await _unitOfWork.MedicalRecords.GetByDateRangeAsync(startDate, endDate);
            return Ok(medicalRecords);
        }

        // GET: api/medicalrecords/number/MR001
        [HttpGet("number/{recordNumber}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecordByNumber(string recordNumber)
        {
            var medicalRecord = await _unitOfWork.MedicalRecords.GetByRecordNumberAsync(recordNumber);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return Ok(medicalRecord);
        }

        // POST: api/medicalrecords
        [HttpPost]
        public async Task<ActionResult<MedicalRecord>> PostMedicalRecord(MedicalRecord medicalRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate record number if not provided
            if (string.IsNullOrEmpty(medicalRecord.RecordNumber))
            {
                medicalRecord.RecordNumber = await GenerateRecordNumber();
            }

            // Set visit date to current date if not provided
            if (medicalRecord.VisitDate == default)
            {
                medicalRecord.VisitDate = DateTime.Now;
            }

            await _unitOfWork.MedicalRecords.AddAsync(medicalRecord);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalRecord), new { id = medicalRecord.Id }, medicalRecord);
        }

        // PUT: api/medicalrecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalRecord(int id, MedicalRecord medicalRecord)
        {
            if (id != medicalRecord.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _unitOfWork.MedicalRecords.UpdateAsync(medicalRecord);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!await MedicalRecordExists(id))
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

        // DELETE: api/medicalrecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var medicalRecord = await _unitOfWork.MedicalRecords.GetByIdAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            await _unitOfWork.MedicalRecords.DeleteAsync(medicalRecord);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> MedicalRecordExists(int id)
        {
            return await _unitOfWork.MedicalRecords.ExistsAsync(mr => mr.Id == id);
        }

        private async Task<string> GenerateRecordNumber()
        {
            var count = await _unitOfWork.MedicalRecords.CountAsync();
            return $"MR{(count + 1):D6}";
        }
    }
}
