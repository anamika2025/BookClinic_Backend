using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
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

        public ClinicTimingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{clinicId}")]
        public async Task<ActionResult<IEnumerable<MstClinicTiming>>> GetTimings(int clinicId)
        {
            return await _context.MstClinicTimings
                .Where(t => t.ClinicId == clinicId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddTiming(MstClinicTiming timing)
        {
            _context.MstClinicTimings.Add(timing);
            await _context.SaveChangesAsync();
            return Ok(timing);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTiming(int id, MstClinicTiming timing)
        {
            if (id != timing.ClinicTimingId)
                return BadRequest();

            _context.Entry(timing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTiming(int id)
        {
            var timing = await _context.MstClinicTimings.FindAsync(id);
            if (timing == null) return NotFound();

            _context.MstClinicTimings.Remove(timing);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
