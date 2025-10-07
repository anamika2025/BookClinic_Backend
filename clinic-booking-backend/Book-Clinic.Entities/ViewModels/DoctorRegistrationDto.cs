using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.ViewModels
{
    public class DoctorRegistrationDto
    {

        public string Name { get; set; }
        public string Specialization { get; set; }
        public int CityId { get; set; }
        public int ClinicId { get; set; }
        public List<string> AvailableDays { get; set; }
        public string AvailableTimeStart { get; set; }
        public string AvailableTimeEnd { get; set; }
    }
}
