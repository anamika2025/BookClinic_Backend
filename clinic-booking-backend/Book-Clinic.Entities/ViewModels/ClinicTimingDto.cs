using Book_Clinic.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.ViewModels
{
    public class ClinicTimingDto
    {
        public int ClinicId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
    }
}
