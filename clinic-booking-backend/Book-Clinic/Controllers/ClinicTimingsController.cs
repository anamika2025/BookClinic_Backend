using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
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
        private readonly ICrudRepository<MstClinicTiming> _clinicRepo;

        public ClinicTimingsController(ApplicationDbContext context, ICrudRepository<MstClinicTiming> clinicRepo)
        {
            _context = context;
            _clinicRepo = clinicRepo;
        }

        [HttpGet("{clinicId}")]
        public async Task<ActionResult<IEnumerable<MstClinicTiming>>> GetTimings(int clinicId)
        {
            return await _context.MstClinicTimings
                .Where(t => t.ClinicId == clinicId)
                .ToListAsync();
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
        public async Task<IActionResult> Add(MstClinicTiming clinic)
        {
            var created = await _clinicRepo.AddAsync(clinic);
            return CreatedAtAction(nameof(GetById), new { id = created.ClinicId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MstClinicTiming clinic)
        {
            if (id != clinic.ClinicId) return BadRequest();
            await _clinicRepo.UpdateAsync(clinic);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clinicRepo.DeleteAsync(id);
            return NoContent();
        }

    }
}
