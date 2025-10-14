using Book_Clinic.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.ViewModels
{
    public class DoctorSlotDto
    {
        public int? SlotId { get; set; }
        public int? DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}