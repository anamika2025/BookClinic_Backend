using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Book_Clinic.Entities.ViewModels;
using Book_Clinic.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrudRepository<Doctor> _doctorRepo;

        public DoctorsController(ApplicationDbContext context, ICrudRepository<Doctor> doctorRepo)
        {
            _context = context;
            _doctorRepo = doctorRepo;
        }


        [HttpGet("api/doctors")]
        public async Task<IActionResult> GetDoctors([FromQuery] int clinicId)
        {
            var doctors = await _context.Doctors
                .Where(d => d.ClinicId == clinicId)
                .Select(d => new { d.DoctorId, d.DoctorName })
                .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _doctorRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            return doctor == null ? NotFound() : Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DoctorDto dto)
        {
            if (dto.ClinicId == null || dto.CityId == null)
                return BadRequest("ClinicId and CityId are required.");

            var cityExists = await _context.Cities.AnyAsync(c => c.CityId == dto.CityId.Value);
            if (!cityExists)
                return BadRequest($"Invalid CityId: {dto.CityId}");

            var clinicExists = await _context.Clinics.AnyAsync(c => c.ClinicId == dto.ClinicId.Value);
            if (!clinicExists)
                return BadRequest($"Invalid ClinicId: {dto.ClinicId}");

            var doctor = new Doctor
            {
                DoctorName = dto.DoctorName,
                CareSpecialization = dto.CareSpecialization,
                CityId = dto.CityId.Value,
                ClinicId = dto.ClinicId.Value,
                Status = dto.Status
            };

            var created = await _doctorRepo.AddAsync(doctor);

            return CreatedAtAction(nameof(GetById), new { id = created.DoctorId }, created);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DoctorDto dto)
        {
            if (id != dto.DoctorId)
                return BadRequest("Mismatched DoctorId");

            var existing = await _doctorRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (dto.ClinicId == null || dto.CityId == null)
                return BadRequest("ClinicId and CityId are required.");

            existing.DoctorName = dto.DoctorName;
            existing.CareSpecialization = dto.CareSpecialization;
            existing.CityId = dto.CityId.Value;
            existing.ClinicId = dto.ClinicId.Value;
            existing.Status = dto.Status;

            await _doctorRepo.UpdateAsync(existing);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _doctorRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _doctorRepo.DeleteAsync(id);
            return NoContent();
        }



    }
}