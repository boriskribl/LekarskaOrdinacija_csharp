using LekarskaOrdinacija.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<MedicalService> MedicalServices => Set<MedicalService>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Nurse> Nurses => Set<Nurse>();
    public DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<MedicalService>().Property(x=>x.Price).HasPrecision(10,2);
        builder.Entity<Appointment>().Property(x=>x.FinalPrice).HasPrecision(10,2);
        builder.Entity<DoctorSchedule>().HasIndex(x=>new{x.DoctorId,x.DayOfWeek,x.StartTime}).IsUnique();
    }
}