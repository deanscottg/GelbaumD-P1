using Barbershop.Models;
using Barbershop.Repositories;

namespace Barbershop.Services
{
   public class BarberService : IBarberService
    {
        private readonly IBarberRepository _repo;

        public BarberService(IBarberRepository repo)
        {
            _repo = repo;
        }

        public async Task CreateAsync(Barber barber)
        {
            await _repo.AddAsync(barber);
            await _repo.SaveChangesAsync();
        }
    }
}