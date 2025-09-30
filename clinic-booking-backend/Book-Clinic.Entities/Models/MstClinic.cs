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
        public string? ClinicName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public string? ClinicAddress { get; set; }
        public int? ContactNumber { get; set; }
        public string? Status { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public ICollection<MstDoctor>? Doctors { get; set; }
    }
}
