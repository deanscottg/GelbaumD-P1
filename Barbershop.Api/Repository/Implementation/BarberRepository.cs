using Barbershop.Models;
using Barbershop.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbershop.Repositories
{
    public class BarberRepository : IBarberRepository
    {
        private readonly BarbershopDbContext _context;

        public BarberRepository(BarbershopDbContext context)
        {
            _context = context;
        }

        public async Task<Barber?> GetByIdAsync(int id)
        {
            //return await _context.Students.Where( student => student.id  == id);
            throw new NotImplementedException();
        }

        public async Task AddAsync(Barber barber)
        {
            await _context.Barbers.AddAsync(barber);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}