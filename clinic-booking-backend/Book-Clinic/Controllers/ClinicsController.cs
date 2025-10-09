using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
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

        public ClinicsController(ICrudRepository<MstClinic> clinicRepo)
        {
            _clinicRepo = clinicRepo;
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
        public async Task<IActionResult> Add(MstClinic clinic)
        {
            var created = await _clinicRepo.AddAsync(clinic);
            return CreatedAtAction(nameof(GetById), new { id = created.ClinicId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MstClinic clinic)
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
