using Barbershop.Models;

namespace Barbershop.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task SaveChangesAsync();
    }
}