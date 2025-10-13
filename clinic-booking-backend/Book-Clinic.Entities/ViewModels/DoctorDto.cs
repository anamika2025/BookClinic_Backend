using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.ViewModels
{
    public class DoctorDto
    {
        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? CareSpecialization { get; set; }
        public string? Status { get; set; }
        public int? ClinicId { get; set; }
        public int? CityId { get; set; }

    }
}
