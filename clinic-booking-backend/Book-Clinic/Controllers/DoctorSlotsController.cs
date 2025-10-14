using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Book_Clinic.Entities.ViewModels;
using Book_Clinic.Repository.IRepository;
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
        private readonly ICrudRepository<DoctorSlot> _repo;

        public DoctorSlotsController(ApplicationDbContext context, ICrudRepository<DoctorSlot> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet("{doctorId}")]
        public async Task<ActionResult<IEnumerable<DoctorSlot>>> GetSlots(int doctorId)
        {
            var slots = await _context.DoctorSlots
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();

            return Ok(slots);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var timings = await _repo.GetAllAsync();
            return Ok(timings);
        }

        [HttpGet("slot/{id}")]
        public async Task<ActionResult<DoctorSlot>> GetById(int id)
        {
            var slot = await _context.DoctorSlots
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SlotId == id);

            if (slot == null)
                return NotFound("Doctor slot not found.");

            return Ok(slot);
        }


        [HttpPost]
        public async Task<ActionResult<DoctorSlot>> AddSlot(DoctorSlotDto dto)
        {
            var doctorExists = await _context.Doctors
                .AnyAsync(d => d.DoctorId == dto.DoctorId && d.Status == "Active");

            if (!doctorExists)
                return BadRequest("Doctor Unavailable.");

            var timeConflict = await _context.DoctorSlots.AnyAsync(s =>
                s.DoctorId == dto.DoctorId &&
                s.DayOfWeek == dto.DayOfWeek &&
                (
                    (dto.FromTime >= s.FromTime && dto.FromTime < s.ToTime) ||
                    (dto.ToTime > s.FromTime && dto.ToTime <= s.ToTime) ||
                    (dto.FromTime <= s.FromTime && dto.ToTime >= s.ToTime)
                )
            );

            if (timeConflict)
                return BadRequest("This time is already taken.");

            var slotBooking = new DoctorSlot
            {
                DoctorId = dto.DoctorId.Value,
                DayOfWeek = dto.DayOfWeek,
                FromTime = dto.FromTime,
                ToTime = dto.ToTime
            };

            var created = await _repo.AddAsync(slotBooking);

            return CreatedAtAction(nameof(GetById), new { id = created.SlotId }, created);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlot(int id, DoctorSlotDto dto)
        {
            var existing = await _context.DoctorSlots.FindAsync(id);
            if (existing == null)
                return NotFound("Slot not found.");

            var doctorExists = await _context.Doctors
                .AnyAsync(d => d.DoctorId == dto.DoctorId && d.Status == "Active");

            if (!doctorExists)
                return BadRequest("Doctor not found or inactive.");

            var overlap = await _context.DoctorSlots.AnyAsync(s =>
                s.SlotId != id &&
                s.DoctorId == dto.DoctorId &&
                s.DayOfWeek == dto.DayOfWeek &&
                (
                    (dto.FromTime >= s.FromTime && dto.FromTime < s.ToTime) ||
                    (dto.ToTime > s.FromTime && dto.ToTime <= s.ToTime) ||
                    (dto.FromTime <= s.FromTime && dto.ToTime >= s.ToTime)
                )
            );

            if (overlap)
                return BadRequest("Time conflict: slot overlaps with another existing slot.");

            // Update properties
            existing.DoctorId = dto.DoctorId.Value;
            existing.DayOfWeek = dto.DayOfWeek;
            existing.FromTime = dto.FromTime;
            existing.ToTime = dto.ToTime;

            await _repo.UpdateAsync(existing);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            var slot = await _context.DoctorSlots.FindAsync(id);
            if (slot == null) return NotFound();
            _context.DoctorSlots.Remove(slot);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}