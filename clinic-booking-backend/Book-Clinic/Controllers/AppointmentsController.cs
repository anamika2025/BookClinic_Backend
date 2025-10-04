using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments([FromQuery] int? doctorId, [FromQuery] int? clinicId, [FromQuery] int? cityId)
        {
            var query = _context.MstAppointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Clinic)
                .Include(a => a.User)
                .AsQueryable();

            if (doctorId.HasValue) query = query.Where(a => a.DoctorId == doctorId.Value);
            if (clinicId.HasValue) query = query.Where(a => a.Doctor.ClinicId == clinicId.Value);
            if (cityId.HasValue) query = query.Where(a => a.CityId == cityId.Value);

            var appointments = await query
                .Select(a => new
                {
                    AppointmentID = a.AppointmentId,
                    DoctorName = a.Doctor.DoctorName,
                    ClinicName = a.Doctor.Clinic.ClinicName,
                    AppointmentDate = a.StartTime.Date,
                    StartTime = a.StartTime.ToString("HH:mm"),
                    EndTime = a.EndTime.ToString("HH:mm"),
                    Status = a.Status
                })
                .ToListAsync();

            return Ok(appointments);
        }
    

    [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] MstAppointment appointment)
        {
            appointment.Status = "Booked";
            _context.MstAppointments.Add(appointment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Appointment booked successfully" });
        }




    }

}
