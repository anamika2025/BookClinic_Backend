using System.ComponentModel.DataAnnotations;

namespace Book_Clinic.Entities.Models
{
    public class MstAppointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int CityId { get; set; }
        public string UserId { get; set; }
        public MstDoctor? Doctor { get; set; }
        public MstUser? User { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Status { get; set; }
    }
}
