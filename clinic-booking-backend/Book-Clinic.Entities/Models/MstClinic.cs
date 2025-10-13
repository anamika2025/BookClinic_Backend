using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.Models
{
    public class MstClinic
    {
        [Key]
        public int ClinicId { get; set; }

        [Required]
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }

        public int CityId { get; set; }
        public MstCity City { get; set; }

        //public int StateId { get; set; }
        //public MstState State { get; set; }

        public long? ContactNumber { get; set; }
        public string Status { get; set; }

        public ICollection<MstDoctor> Doctors { get; set; }
        public ICollection<MstAppointment> Appointments { get; set; }

        public ICollection<MstClinicTiming> Timings { get; set; }
    }
}
