using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.Models
{
    public class DoctorSlot
    {
        [Key]
        public int SlotId { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan FromTime { get; set; }

        [Required]
        public TimeSpan ToTime { get; set; }
    }
}