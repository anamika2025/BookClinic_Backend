using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.Models
{
    public class MstState
    {
        [Key]

        public int StateId { get; set; }
        public string StateName { get; set; }

        public ICollection<MstCity> Cities { get; set; }
    }
}
