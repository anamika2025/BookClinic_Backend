using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Book_Clinic.Entities.ViewModels;
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

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
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

        [HttpPost("api/doctors/register")]
        public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegistrationDto doctorDto)
        {
            var doctor = new MstDoctor
            {
                DoctorName = doctorDto.Name,
                CareSpecialization = doctorDto.Specialization,
                CityId = doctorDto.CityId,
                ClinicId = doctorDto.ClinicId,
            };

            _context.MstDoctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Doctor registered successfully" });
        }


        //[HttpGet]
        //public async Task<IActionResult> GetDoctorsbyCity([FromQuery] int cityid)
        //{
        //    var doctors = await _context.MstDoctors
        //        .Where(d => d.CityId == cityid)
        //        .Select(d => new
        //        {
        //            DoctorID = d.DoctorId,
        //            DoctorName = d.DoctorName
        //        })
        //        .ToListAsync();

        //    return Ok(doctors);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllDoctors()
        //{
        //    var doctors = await _context.MstDoctors
        //        .Where(d => d.Status == "Active")
        //        .Select(d => new
        //        {
        //            DoctorID = d.DoctorId,
        //            DoctorName = d.DoctorName
        //        })
        //        .ToListAsync();

        //    return Ok(doctors);
        //}



    }
}
