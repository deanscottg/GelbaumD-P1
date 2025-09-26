using Barbershop.Models;
using Barbershop.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbershop.Repositories
{   
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BarbershopDbContext _context;

        public AppointmentRepository(BarbershopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Barbers)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Barbers)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public void Remove(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
        }

        public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId)
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Barbers)
                .Where(a => a.Barbers.Any(b => b.Id == barberId))
                .ToListAsync();
        }

        public async Task<bool> BarberHasAppointmentAtTimeAsync(int barberId, DateTime time, int? excludeAppointmentId = null)
        {
            return await _context.Appointments
                .Include(a => a.Barbers)
                .AnyAsync(a =>
                    a.AppointmentDateAndTime == time &&
                    a.Barbers.Any(b => b.Id == barberId) &&
                    (!excludeAppointmentId.HasValue || a.Id != excludeAppointmentId.Value));
        }
        public async Task<Barber?> GetBarberByIdAsync(int id)
        {
            return await _context.Barbers.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}