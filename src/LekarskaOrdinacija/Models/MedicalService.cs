using System.ComponentModel.DataAnnotations;
namespace LekarskaOrdinacija.Models;
public class MedicalService
{
    public int Id { get; set; }
    [Required, StringLength(120)] public string Name { get; set; } = "";
    [Range(0.01, 1000000)] public decimal Price { get; set; }
    [Range(5, 480)] public int DurationMinutes { get; set; }
    [StringLength(500)] public string? Description { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}