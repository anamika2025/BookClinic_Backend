using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? CareSpecialization { get; set; }
        public string? Status { get; set; }
        public int ClinicId { get; set; }
        public int CityId { get; set; }
        public Clinic? Clinic { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<DoctorSlot> WorkingSlots { get; set; }
    }
}