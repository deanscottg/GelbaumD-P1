using Barbershop.Models;

namespace Barbershop.Services
{
    public interface IBarberService
    {
        
        // Create Barber
        public Task CreateAsync(Barber barber);

        // Get all  appointments by barberid
        // public Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId);
        

    }
}