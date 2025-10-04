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


        [HttpGet]
        public async Task<IActionResult> GetClinics([FromQuery] int cityId)
        {
            var clinics = await _context.MstClinic
                .Where(c => c.CityId == cityId)
                .Select(c => new
                {
                    ClinicID = c.ClinicId,  
                    ClinicName = c.ClinicName
                })
                .ToListAsync();

            return Ok(clinics);
        }





    }
}
