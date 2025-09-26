using Barbershop.Models;
using Barbershop.Repositories;

namespace Barbershop.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<Customer?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CreateAsync(Customer customer)
        {
            await _repo.AddAsync(customer);
            await _repo.SaveChangesAsync();
        }


    }
}