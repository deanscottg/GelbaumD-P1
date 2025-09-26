using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Barbershop.Models;

public class Barber 
{
   
    public int Id { get; set; }
    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    public List<Appointment> Appointments { get; set; } = new();

}