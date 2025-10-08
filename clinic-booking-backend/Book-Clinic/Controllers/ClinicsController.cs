using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClinicsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("api/clinics")]
        public async Task<IActionResult> GetClinicsByCity([FromQuery] int cityId)
        {
            var clinics = await _context.MstClinics
                .Where(c => c.CityId == cityId)
                .Select(c => new { c.ClinicId, c.ClinicName })
                .ToListAsync();

            return Ok(clinics);
        }

        [HttpGet("api/clinics/{id}")]
        public async Task<IActionResult> GetClinicById(int id)
        {
            var clinic = await _context.MstClinics
                .Include(c => c.City)
                .Include(c => c.State)
                .Include(c => c.Doctors)
                .Include(c => c.Timings)
                .FirstOrDefaultAsync(c => c.ClinicId == id);

            if (clinic == null) return NotFound();

            return Ok(clinic);
        }

        [HttpPost("api/clinics")]
        public async Task<IActionResult> CreateClinic([FromBody] MstClinic clinic)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.MstClinics.Add(clinic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClinicById), new { id = clinic.ClinicId }, clinic);
        }


        [HttpPut("api/clinics/{id}")]
        public async Task<IActionResult> UpdateClinic(int id, [FromBody] MstClinic clinic)
        {
            if (id != clinic.ClinicId)
                return BadRequest("Clinic ID mismatch.");

            var existingClinic = await _context.MstClinics.FindAsync(id);
            if (existingClinic == null)
                return NotFound();

            existingClinic.ClinicName = clinic.ClinicName;
            existingClinic.ClinicAddress = clinic.ClinicAddress;
            existingClinic.CityId = clinic.CityId;
            existingClinic.StateId = clinic.StateId;
            existingClinic.ContactNumber = clinic.ContactNumber;
            existingClinic.Status = clinic.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }




        [HttpDelete("api/clinics/{id}")]
        public async Task<IActionResult> DeleteClinic(int id)
        {
            var clinic = await _context.MstClinics.FindAsync(id);
            if (clinic == null)
                return NotFound();

            _context.MstClinics.Remove(clinic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        




    }
}
