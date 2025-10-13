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
        private readonly ICrudRepository<MstClinicTiming> _repo;

        public ClinicTimingsController(ApplicationDbContext context, ICrudRepository<MstClinicTiming> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet("{clinicId}")]
        public async Task<IActionResult> GetByClinic(int clinicId)
        {
            var timings = await _context.MstClinicTimings
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
            var timing = await _context.MstClinicTimings
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.ClinicTimingId == id);

            if (timing == null)
                return NotFound();

            return Ok(timing);
        }

    
        [HttpPost]
        public async Task<IActionResult> Add(ClinicTimingDto dto)
        {
            var clinicExists = await _context.MstClinics
                .AnyAsync(c => c.ClinicId == dto.ClinicId && c.Status == "Active");

            if (!clinicExists)
                return BadRequest("Invalid or inactive ClinicId.");

            if (dto.OpeningTime >= dto.ClosingTime)
                return BadRequest("OpeningTime must be before ClosingTime.");

            var clinicTiming = new MstClinicTiming
            {
                ClinicId = dto.ClinicId,
                DayOfWeek = dto.DayOfWeek,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime,
            };

            var created = await _repo.AddAsync(clinicTiming);

            return CreatedAtAction(nameof(GetById), new { id = created.ClinicTimingId }, created);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MstClinicTiming timing)
        {
            if (id != timing.ClinicTimingId)
                return BadRequest("Id mismatch.");

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var clinic = await _context.MstClinics
                .Where(c => c.ClinicId == timing.ClinicId && c.Status == "Active")
                .FirstOrDefaultAsync();

            if (clinic == null)
                return BadRequest("Invalid or inactive ClinicId.");

            if (timing.OpeningTime >= timing.ClosingTime)
                return BadRequest("OpeningTime must be before ClosingTime.");

            existing.ClinicId = timing.ClinicId;
            existing.DayOfWeek = timing.DayOfWeek;
            existing.OpeningTime = timing.OpeningTime;
            existing.ClosingTime = timing.ClosingTime;

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
