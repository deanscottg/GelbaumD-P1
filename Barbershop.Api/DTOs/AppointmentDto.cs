namespace Barbershop.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime AppointmentDateAndTime { get; set; }
        public string HaircutType { get; set; } = string.Empty;
         public List<BarberDto> Barbers { get; set; } = new();
    }
}