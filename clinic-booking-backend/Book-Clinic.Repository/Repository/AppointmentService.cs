using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Repository.Repository
{
    public class AppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> BookAppointmentAsync(int doctorId, int clinicId, DateTime startTime, DateTime endTime, string userId)
        {
            // Check if doctor is available
            bool isBooked = await _context.MstAppointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.Status == "Booked" &&
                ((startTime >= a.StartTime && startTime < a.EndTime) ||
                 (endTime > a.StartTime && endTime <= a.EndTime)));

            if (isBooked)
                return "Slot already booked!";

            var appointment = new MstAppointment
            {
                DoctorId = doctorId,
                ClinicId = clinicId,
                StartTime = startTime,
                EndTime = endTime,
                UserId = userId,
                Status = "Booked"
            };

            _context.MstAppointments.Add(appointment);
            await _context.SaveChangesAsync();

            return "Appointment booked successfully.";
        }


    }
}
