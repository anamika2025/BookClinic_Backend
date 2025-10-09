using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.ViewModels
{
    public class AppointmentRequestDto
    {
        public int DoctorId { get; set; }
        public int ClinicId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? UserId { get; set; }
    }
}
