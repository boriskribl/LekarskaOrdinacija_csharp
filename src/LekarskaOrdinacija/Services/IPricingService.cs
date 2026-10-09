using LekarskaOrdinacija.Models;
namespace LekarskaOrdinacija.Services;
public interface IPricingService { decimal Calculate(Patient patient, MedicalService service, DateTime serviceDate); }