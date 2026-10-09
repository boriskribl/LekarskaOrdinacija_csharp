using System.ComponentModel.DataAnnotations;
namespace LekarskaOrdinacija.Models;
public class Nurse
{
    public int Id { get; set; }
    [Required, StringLength(120)] public string FullName { get; set; } = "";
    [Phone, StringLength(30)] public string? Phone { get; set; }
    [EmailAddress, StringLength(120)] public string? Email { get; set; }
}