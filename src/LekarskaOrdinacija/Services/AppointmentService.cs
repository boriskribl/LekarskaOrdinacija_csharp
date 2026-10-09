using LekarskaOrdinacija.Data;
using LekarskaOrdinacija.Models;
using LekarskaOrdinacija.Repositories;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Services;
public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _db; private readonly IAppointmentRepository _repo; private readonly IPricingService _pricing;
    public AppointmentService(ApplicationDbContext db,IAppointmentRepository repo,IPricingService pricing){_db=db;_repo=repo;_pricing=pricing;}
    public async Task<(bool ok,string? error)> ValidateAndPriceAsync(Appointment a)
    {
        var patient=await _db.Patients.FindAsync(a.PatientId); var service=await _db.MedicalServices.FindAsync(a.MedicalServiceId);
        if(patient is null || service is null) return(false,"Pacijent ili usluga ne postoji.");
        var schedule=await _db.DoctorSchedules.FirstOrDefaultAsync(s=>s.DoctorId==a.DoctorId && s.DayOfWeek==a.Start.DayOfWeek);
        if(schedule is null) return(false,"Lekar tog dana nema definisano radno vreme.");
        var end=a.Start.AddMinutes(service.DurationMinutes); var t=a.Start.TimeOfDay;
        if(t<schedule.StartTime || end.TimeOfDay>schedule.EndTime) return(false,"Termin je van radnog vremena lekara.");
        if(await _repo.HasConflictAsync(a.DoctorId,a.Start,end,a.Id==0?null:a.Id)) return(false,"Lekar već ima termin u izabranom vremenu.");
        a.FinalPrice=_pricing.Calculate(patient,service,a.Start);
        return(true,null);
    }
}