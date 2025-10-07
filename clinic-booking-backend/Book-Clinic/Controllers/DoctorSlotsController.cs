using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Book_Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSlotsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorSlotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{doctorId}")]
        public async Task<ActionResult<IEnumerable<MstDoctorSlot>>> GetSlots(int doctorId)
        {
            var slots = await _context.MstDoctorSlots
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();

            return Ok(slots);
        }

        [HttpPost]
        public async Task<ActionResult<MstDoctorSlot>> AddSlot(MstDoctorSlot slot)
        {
            _context.MstDoctorSlots.Add(slot);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSlots), new { doctorId = slot.DoctorId }, slot);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlot(int id, MstDoctorSlot slot)
        {
            if (id != slot.SlotId) return BadRequest();
            _context.Entry(slot).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            var slot = await _context.MstDoctorSlots.FindAsync(id);
            if (slot == null) return NotFound();
            _context.MstDoctorSlots.Remove(slot);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{doctorId}")]
        public async Task<IActionResult> SaveSlots(int doctorId, [FromBody] List<MstDoctorSlot> slots)
        {
            var doctor = await _context.MstDoctors.Include(d => d.WorkingSlots)
                                               .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
            if (doctor == null)
                return NotFound("Doctor not found");

            // Remove old slots (optional if you want to overwrite)
            _context.MstDoctorSlots.RemoveRange(doctor.WorkingSlots);

            // Add new slots
            foreach (var slot in slots)
            {
                slot.SlotId = 0; // ensure new insert
                slot.DoctorId = doctorId;
                _context.MstDoctorSlots.Add(slot);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Slots saved successfully" });
        }
    }
}
