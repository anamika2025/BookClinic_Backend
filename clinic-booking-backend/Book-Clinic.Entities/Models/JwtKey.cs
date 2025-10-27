using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.Models
{
    public class JwtKey
    {
        [Key]
        public int Id { get; set; }
        public string KeyName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
