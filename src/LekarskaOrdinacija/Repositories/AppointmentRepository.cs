using LekarskaOrdinacija.Data;
using LekarskaOrdinacija.Models;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Repositories;
public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext db):base(db){}
    public Task<List<Appointment>> GetDetailedAsync()=>Db.Appointments.AsNoTracking()
        .Include(x=>x.Patient).Include(x=>x.Doctor).Include(x=>x.MedicalService)
        .OrderBy(x=>x.Start).ToListAsync();
    public Task<bool> HasConflictAsync(int doctorId, DateTime start, DateTime end, int? excludeId=null)=>
        Db.Appointments.Include(x=>x.MedicalService).AnyAsync(x=>x.DoctorId==doctorId && x.Status!=AppointmentStatus.Cancelled &&
            (!excludeId.HasValue || x.Id!=excludeId.Value) && start < x.Start.AddMinutes(x.MedicalService!.DurationMinutes) && end > x.Start);
}