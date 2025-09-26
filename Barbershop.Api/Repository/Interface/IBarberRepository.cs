using Barbershop.Models;

namespace Barbershop.Repositories
{
    public interface IBarberRepository
    {
        public Task AddAsync(Barber barber);
        public Task SaveChangesAsync();
    }
}