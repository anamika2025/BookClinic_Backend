using System.ComponentModel.DataAnnotations;

namespace Book_Clinic.Entities.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public int? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }

        public string? UserId { get; set; }
        public User User { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Booked";
    }
}


