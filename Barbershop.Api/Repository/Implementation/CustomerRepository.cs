using Microsoft.EntityFrameworkCore;
using Barbershop.Models;
using Barbershop.Data;
namespace Barbershop.Repositories

{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BarbershopDbContext _context;

        public CustomerRepository(BarbershopDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Appointments)
                .ThenInclude(a => a.Barbers)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
