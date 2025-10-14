using Book_Clinic.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/search")]
        public async Task<ActionResult<dynamic>> Search([FromQuery] string query)
        {
            try
            {
                var doctors = await _context.Doctors
                    .Where(d => EF.Functions.Like(d.DoctorName.ToLower(), $"%{query.ToLower()}%"))
                    .ToListAsync();

                var clinics = await _context.Clinics
                    .Where(c => EF.Functions.Like(c.ClinicName.ToLower(), $"%{query.ToLower()}%"))
                    .ToListAsync();

                return new { doctors, clinics };
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}