using Barbershop.Models;

namespace Barbershop.Services
{
    public interface ICustomerService
    {
        // Get a Customer by Id
        public Task<Customer?> GetByIdAsync(int id);
        // Create a Customer
        public Task CreateAsync(Customer customer);
        // Get all the appointments by a custimer Id
        // public Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId);
    }
}