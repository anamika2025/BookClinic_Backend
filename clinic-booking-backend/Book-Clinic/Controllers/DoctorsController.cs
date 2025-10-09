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
        private readonly ICrudRepository<MstDoctor> _clinicRepo;

        public DoctorsController(ApplicationDbContext context, ICrudRepository<MstDoctor> clinicRepo)
        {
            _context = context;
            _clinicRepo = clinicRepo;
        }


        [HttpGet("api/doctors")]
        public async Task<IActionResult> GetDoctors([FromQuery] int clinicId)
        {
            var doctors = await _context.MstDoctors
                .Where(d => d.ClinicId == clinicId)
                .Select(d => new { d.DoctorId, d.DoctorName })
                .ToListAsync();

            return Ok(doctors);
        }

        //[HttpPost("api/doctors/register")]
        //public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegistrationDto doctorDto)
        //{
        //    var doctor = new MstDoctor
        //    {
        //        DoctorName = doctorDto.Name,
        //        CareSpecialization = doctorDto.Specialization,
        //        CityId = doctorDto.CityId,
        //        ClinicId = doctorDto.ClinicId,
        //    };

        //    _context.MstDoctors.Add(doctor);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { message = "Doctor registered successfully" });
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _clinicRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var clinic = await _clinicRepo.GetByIdAsync(id);
            return clinic == null ? NotFound() : Ok(clinic);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MstDoctor mstDoctor)
        {
            var created = await _clinicRepo.AddAsync(mstDoctor);
            return CreatedAtAction(nameof(GetById), new { id = created.DoctorId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MstDoctor mstDoctor)
        {
            if (id != mstDoctor.DoctorId) return BadRequest();
            await _clinicRepo.UpdateAsync(mstDoctor);
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
