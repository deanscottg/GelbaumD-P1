using Barbershop.Models;

namespace Barbershop.Repositories
{
    public interface IAppointmentRepository
    {
        public Task<List<Appointment>> GetAllAsync();
        public Task<Appointment?> GetByIdAsync(int id);
        public Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId);
        public Task AddAsync(Appointment appointment);
        void Remove(Appointment appointment);
        public Task SaveChangesAsync();
        void Update(Appointment appointment);
        // Check to see if a barber has an appointment at that time
        public Task<bool> BarberHasAppointmentAtTimeAsync(int barberId, DateTime time, int? excludeAppointmentId = null);
        public Task<Barber?> GetBarberByIdAsync(int id);

    }
}