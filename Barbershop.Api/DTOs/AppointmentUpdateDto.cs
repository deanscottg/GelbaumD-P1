using Barbershop.Models;


namespace Barbershop.DTOs
{
    public class AppointmentUpdateDto
    {
        public int BarberId { get; set; }
        public DateTime AppointmentDateAndTime { get; set; }
        public HaircutType HaircutType { get; set; } 
    }
}