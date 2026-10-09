using LekarskaOrdinacija.Models;
namespace LekarskaOrdinacija.Services;
public class PricingService : IPricingService
{
    public decimal Calculate(Patient patient, MedicalService service, DateTime serviceDate)
    {
        var age=serviceDate.Year-patient.DateOfBirth.Year;
        if(patient.DateOfBirth.Date>serviceDate.AddYears(-age).Date) age--;
        return age<12 ? Math.Round(service.Price*0.75m,2) : service.Price;
    }
}