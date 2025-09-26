using Barbershop.Models;

namespace Barbershop.DTOs
{
    public class AppointmentCreateDto
    {
        public DateTime AppointmentDateAndTime { get; set; }
        public HaircutType HaircutType { get; set; }

        // Customer assigning by Id
        public int CustomerId { get; set; }

        // Multiple barbers by their Ids
        public List<int> BarberIds { get; set; } = new();
    }
}