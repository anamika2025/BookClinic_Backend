using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Book_Clinic.Entities.ViewModels;
using Book_Clinic.Repository.IRepository;
using Book_Clinic.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly ICrudRepository<MstClinic> _clinicRepo;
        private readonly ApplicationDbContext _context;

        public ClinicsController(ICrudRepository<MstClinic> clinicRepo, ApplicationDbContext context)
        {
            _clinicRepo = clinicRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _clinicRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var clinic = await _clinicRepo.GetByIdAsync(id);
            return clinic == null ? NotFound() : Ok(clinic);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClinicCreateDto dto)
        {
            if (dto.CityId.HasValue)
            {
                var cityExists = await _context.MstCity.AnyAsync(c => c.CityId == dto.CityId.Value);
                if (!cityExists)
                    return BadRequest($"Invalid CityId: {dto.CityId}. City does not exist.");
            }

            var clinic = new MstClinic
            {
                ClinicName = dto.ClinicName,
                ClinicAddress = dto.ClinicAddress,
                CityId = (int)dto.CityId,
                ContactNumber = dto.ContactNumber,
                Status = dto.Status
            };

            if (dto.CityId.HasValue)
                clinic.CityId = dto.CityId.Value;

            var created = await _clinicRepo.AddAsync(clinic);

            return CreatedAtAction(nameof(GetById), new { id = created.ClinicId }, created);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClinicCreateDto clinic)
        {
            if (id != clinic.ClinicId) return BadRequest();

            var existingClinic = await _clinicRepo.GetByIdAsync(id);
            if (existingClinic == null) return NotFound();

            // Map DTO to entity
            existingClinic.ClinicName = clinic.ClinicName;
            existingClinic.ClinicAddress = clinic.ClinicAddress;
            existingClinic.CityId = (int)clinic.CityId;
            existingClinic.ContactNumber = clinic.ContactNumber;
            existingClinic.Status = clinic.Status;

            await _clinicRepo.UpdateAsync(existingClinic);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clinicRepo.DeleteAsync(id);
            return NoContent();
        }

        //[HttpGet("api/clinics")]
        //public async Task<IActionResult> GetClinicsByCity([FromQuery] int cityId)
        //{
        //    var clinics = await _clinicRepo.MstClinic
        //        .Where(c => c.CityId == cityId)
        //        .Select(c => new { c.ClinicId, c.ClinicName })
        //        .ToListAsync();

        //    return Ok(clinics);
        //}

        

    }
}
