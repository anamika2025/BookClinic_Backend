using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.ViewModels
{
    public class ClinicCreateDto
    {
        public int? ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public int? CityId { get; set; }
        //public int? StateId { get; set; }
        public long? ContactNumber { get; set; }
        public string? Status { get; set; }
    }
}
