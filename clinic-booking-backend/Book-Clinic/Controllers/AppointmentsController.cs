using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Book_Clinic.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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


        //[Authorize]
        [HttpPost("CreateAppointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequestDto dto)
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userId == null)
            //    return Unauthorized();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                ClinicId = dto.ClinicId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                UserId = userId, 
                Status = dto.Status 
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment Booked Successfully" });
        }


        [HttpGet("{clinicId}/{doctorId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments(int clinicId, int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.ClinicId == clinicId && a.DoctorId == doctorId)
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId) return BadRequest();
            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }





    }


}