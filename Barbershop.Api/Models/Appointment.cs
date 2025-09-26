using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Barbershop.Attributes;

namespace Barbershop.Models;

public class Appointment
{

    public int Id { get; set; }

    [Required]
    [AppointmentHours(9, 17)]
    public DateTime AppointmentDateAndTime { get; set; }

    [Required]
    public HaircutType HaircutType { get; set; }

    public List<Barber> Barbers { get; set; } = new();

    [Required]
    public int CustomerId { get; set; }

    public Customer Customer { get; set; }
}