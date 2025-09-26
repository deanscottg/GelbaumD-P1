using Barbershop.Models;

namespace Barbershop.Services
{
    public interface IAppointmentService
    {
        
        // List all appointments by day? 
        // Get all appointments
        public Task<List<Appointment>> GetAllAsync();
        // Get an appointment by the AppointmentId
        public Task<Appointment?> GetByIdAsync(int id);
        // Get all appointments by Barber
        public Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId);
        // Create an appointment
        public Task CreateAsync(Appointment appointment);
        // Delete an appointment
        public Task<bool> DeleteAsync(int id);
        // Update existing appointment by barber or time only
       Task<bool> UpdateAsync(int appointmentId, int newBarberId, DateTime newAppointmentTime, HaircutType newHaircutType);
    }
}