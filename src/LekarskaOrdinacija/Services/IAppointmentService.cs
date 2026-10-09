using LekarskaOrdinacija.Models;
namespace LekarskaOrdinacija.Services;
public interface IAppointmentService { Task<(bool ok,string? error)> ValidateAndPriceAsync(Appointment appointment); }