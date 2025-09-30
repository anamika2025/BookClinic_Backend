using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Entities.Models;

public class MstUser : IdentityUser
{
    [Key]
    public int UserId { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }

    public int CityId { get; set; }

    public ICollection<MstAppointment>? Appointments { get; set; }

}


