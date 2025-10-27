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
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Clinic> Clinics { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<JwtKey> JwtKeys { get; set; }

        public DbSet<Entities.Models.Appointment> Appointments { get; set; }

        public DbSet<ClinicTiming> ClinicTimings { get; set; }

        public DbSet<DoctorSlot> DoctorSlots { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
        .HasOne(a => a.Clinic)
        .WithMany(c => c.Appointments)
        .HasForeignKey(a => a.ClinicId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
        .HasOne(a => a.User)
        .WithMany(u => u.Appointments)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
         .HasOne(a => a.User)
         .WithMany(u => u.Appointments)
         .HasForeignKey(a => a.UserId)
         .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Clinic>()
        .HasOne(c => c.City)
        .WithMany(city => city.Clinics)
        .HasForeignKey(c => c.CityId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<City>()
        .HasOne(c => c.State)
        .WithMany(s => s.Cities)
        .HasForeignKey(c => c.StateId)
        .OnDelete(DeleteBehavior.Restrict);
        }


    }
}