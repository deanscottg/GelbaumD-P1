
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Barbershop.Models;

public class Customer 
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; }
   
    [Required]
    [MaxLength(20)]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; } 

    public List<Appointment> Appointments { get; set; } = new();

}
