using System.ComponentModel.DataAnnotations;
namespace LekarskaOrdinacija.Models;
public class Doctor
{
    public int Id { get; set; }
    [Required, StringLength(120)] public string FullName { get; set; } = "";
    [Required, StringLength(120)] public string Specialization { get; set; } = "";
    [Phone, StringLength(30)] public string? Phone { get; set; }
    [EmailAddress, StringLength(120)] public string? Email { get; set; }
    public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}