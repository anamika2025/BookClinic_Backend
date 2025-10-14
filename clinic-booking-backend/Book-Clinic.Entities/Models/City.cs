using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.Models
{
    public class City
    {
        [Key]

        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }

        public ICollection<Clinic> Clinics { get; set; }
    }
}