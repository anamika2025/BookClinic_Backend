using Book_Clinic.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Clinic.Data
{
    public class ApplicationDbContext : IdentityDbContext<MstUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<MstClinic> MstClinics { get; set; }

        public DbSet<MstCity> MstCity { get; set; }

        public DbSet<MstState> MstStates { get; set; }

        public DbSet<MstDoctor> MstDoctors { get; set; }

        public DbSet<MstAppointment> MstAppointments { get; set; }

        public DbSet<MstClinicTiming> MstClinicTimings { get; set; }

        public DbSet<MstDoctorSlot> MstDoctorSlots { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MstAppointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MstAppointment>()
        .HasOne(a => a.Clinic)
        .WithMany(c => c.Appointments)
        .HasForeignKey(a => a.ClinicId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MstAppointment>()
        .HasOne(a => a.User)
        .WithMany(u => u.Appointments)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MstAppointment>()
         .HasOne(a => a.User)
         .WithMany(u => u.Appointments)
         .HasForeignKey(a => a.UserId)
         .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MstClinic>()
        .HasOne(c => c.City)
        .WithMany(city => city.Clinics)
        .HasForeignKey(c => c.CityId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MstCity>()
        .HasOne(c => c.State)
        .WithMany(s => s.Cities)
        .HasForeignKey(c => c.StateId)
        .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
