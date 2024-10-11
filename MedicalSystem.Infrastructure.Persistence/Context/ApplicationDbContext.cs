using MedicalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        #region "DbSets"
        public DbSet<User> Users { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<LabTest> labTests { get; set; }
        public DbSet<LabResult> labResults { get; set; }
        #endregion

        #region "Configuration"
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Appointment>()
                .HasOne(c => c.Doctor)
                .WithMany(m => m.Appointments)
                .HasForeignKey(c => c.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Appointment>()
                .HasOne(c => c.Patient)
                .WithMany(m => m.Appointments)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<LabResult>()
                .HasOne(c => c.LabTest)
                .WithMany(m => m.Results)
                .HasForeignKey(r => r.LabTestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabResult>()
                .HasOne(c => c.Patient)
                .WithMany(m => m.Results)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<LabTest>()
                .HasOne<Clinic>()
                .WithMany()
                .HasForeignKey(l => l.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Appointment>()
             .HasOne<Clinic>()
             .WithMany()
             .HasForeignKey(a => a.ClinicId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabResult>()
             .HasOne<Clinic>()
             .WithMany()
             .HasForeignKey(l => l.ClinicId)
             .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
