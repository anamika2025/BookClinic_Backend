using Book_Clinic.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SlotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAvailableSlots([FromQuery] string? city)
        //{
        //    // Step 1: Fetch active clinics (filter by city if provided)
        //    var clinicsQuery = _context.Clinics
        //        .Where(c => c.Status == "Active")
        //        .Include(c => c.ClinicName);

        //    if (!string.IsNullOrEmpty(city))
        //    {
        //        // If Clinic.City is a string:
        //        clinicsQuery = clinicsQuery.Where(c => c.City == city);
        //        // If Clinic.City is a navigation property, change to:
        //        // clinicsQuery = clinicsQuery.Include(c => c.City)
        //        //                            .Where(c => c.City.Name == city);
        //    }

        //    var clinics = await clinicsQuery.ToListAsync();

        //    // Step 2: Gather doctor slots for these clinics
        //    var doctorSlots = await _context.DoctorSlots
        //        .Include(s => s.Doctor)
        //        .Where(s => s.Doctor.Status == "Active")
        //        .ToListAsync();

        //    // Step 3: Build available slot data
        //    var availableSlots = new List<object>();

        //    foreach (var clinic in clinics)
        //    {
        //        foreach (var timing in clinic.ClinicTiming)
        //        {
        //            var doctorsInClinic = _context.Doctors
        //                .Where(d => d.ClinicId == clinic.ClinicId && d.Status == "Active")
        //                .ToList();

        //            foreach (var doctor in doctorsInClinic)
        //            {
        //                var docSlots = doctorSlots
        //                    .Where(s => s.DoctorId == doctor.DoctorId && s.DayOfWeek == timing.DayOfWeek)
        //                    .ToList();

        //                foreach (var slot in docSlots)
        //                {
        //                    // Example available slot output
        //                    availableSlots.Add(new
        //                    {
        //                        ClinicId = clinic.ClinicId,
        //                        ClinicName = clinic.ClinicName,
        //                        DoctorId = doctor.DoctorId,
        //                        DoctorName = doctor.DoctorName,
        //                        DayOfWeek = slot.DayOfWeek,
        //                        FromTime = slot.FromTime,
        //                        ToTime = slot.ToTime
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return Ok(availableSlots);
        //}
    }
}
