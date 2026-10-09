using LekarskaOrdinacija.Models;
namespace LekarskaOrdinacija.Repositories;
public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<List<Appointment>> GetDetailedAsync();
    Task<bool> HasConflictAsync(int doctorId, DateTime start, DateTime end, int? excludeId=null);
}