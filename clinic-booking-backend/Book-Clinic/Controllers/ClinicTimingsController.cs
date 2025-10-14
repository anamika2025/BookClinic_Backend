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
    public class ClinicTimingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrudRepository<ClinicTiming> _repo;

        public ClinicTimingsController(ApplicationDbContext context, ICrudRepository<ClinicTiming> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet("clinic/{clinicId:int}")]
        public async Task<IActionResult> GetByClinic(int clinicId)
        {
            var timings = await _context.ClinicTimings
                .Where(t => t.ClinicId == clinicId)
                .ToListAsync();
            return Ok(timings);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var timings = await _repo.GetAllAsync();
            return Ok(timings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var timing = await _context.ClinicTimings
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.ClinicTimingId == id);

            if (timing == null)
                return NotFound();

            return Ok(timing);
        }


        [HttpPost]
        public async Task<IActionResult> Add(ClinicTimingDto dto)
        {
            var clinicExists = await _context.Clinics
                .AnyAsync(c => c.ClinicId == dto.ClinicId && c.Status == "Active");

            if (!clinicExists)
                return BadRequest("Invalid or inactive ClinicId.");

            if (dto.OpeningTime >= dto.ClosingTime)
                return BadRequest("OpeningTime must be before ClosingTime.");

            var clinicTiming = new ClinicTiming
            {
                ClinicId = (int)dto.ClinicId,
                DayOfWeek = dto.DayOfWeek,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime,
            };

            var created = await _repo.AddAsync(clinicTiming);

            return CreatedAtAction(nameof(GetById), new { id = created.ClinicTimingId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClinicTimingDto dto)
        {
            if (id != dto.ClinicTimingId)
                return BadRequest("Id mismatch.");

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var clinic = await _context.Clinics
               .Where(c => c.ClinicId == dto.ClinicId && c.Status == "Active")
               .FirstOrDefaultAsync();

            if (clinic == null)
                return BadRequest("Invalid or inactive ClinicId.");

            if (dto.OpeningTime >= dto.ClosingTime)
                return BadRequest("OpeningTime must be before ClosingTime.");

            existing.ClinicId = (int)dto.ClinicId;
            existing.DayOfWeek = dto.DayOfWeek;
            existing.OpeningTime = dto.OpeningTime;
            existing.ClosingTime = dto.ClosingTime;

            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _repo.DeleteAsync(id);
            return NoContent();
        }

    }
}