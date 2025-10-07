using Book_Clinic.Data;
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






    }
}
