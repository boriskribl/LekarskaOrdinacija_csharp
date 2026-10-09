using System.ComponentModel.DataAnnotations;
namespace LekarskaOrdinacija.Models;
public class Patient
{
    public int Id { get; set; }
    [Required, StringLength(60)] public string FirstName { get; set; } = "";
    [Required, StringLength(60)] public string LastName { get; set; } = "";
    [StringLength(150)] public string? Address { get; set; }
    [DataType(DataType.Date)] public DateTime DateOfBirth { get; set; }
    [Phone, StringLength(30)] public string? Phone { get; set; }
    [EmailAddress, StringLength(120)] public string? Email { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public string FullName => $"{FirstName} {LastName}";
}