using System.ComponentModel.DataAnnotations;
namespace LekarskaOrdinacija.Models;
public enum AppointmentStatus { Scheduled, Completed, Cancelled }
public class Appointment
{
    public int Id { get; set; }
    [Required] public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    [Required] public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    [Required] public int MedicalServiceId { get; set; }
    public MedicalService? MedicalService { get; set; }
    [Required] public DateTime Start { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    [Range(0,1000000)] public decimal FinalPrice { get; set; }
    [StringLength(1000)] public string? Note { get; set; }
}