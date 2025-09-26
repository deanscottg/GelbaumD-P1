using Barbershop.Models;
using Barbershop.Repositories;

namespace Barbershop.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        
        }

        public async Task<List<Appointment>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Appointment?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CreateAsync(Appointment appointment)
        {
    // Ask the repository for existing appointments instead of using _context
            var allAppointments = await _repo.GetAllAsync();
            bool conflict = allAppointments.Any(a =>
                a.AppointmentDateAndTime == appointment.AppointmentDateAndTime &&
                a.Barbers.Any(b => appointment.Barbers.Select(bb => bb.Id).Contains(b.Id))
            );

            if (conflict)
                throw new InvalidOperationException("A barber is already booked at that time.");

            await _repo.AddAsync(appointment);
            await _repo.SaveChangesAsync();
        }

        // Delete an appointment??
        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _repo.GetByIdAsync(id);
            if(appointment == null)
                return false;
            
            _repo.Remove(appointment);
            await _repo.SaveChangesAsync();
            return true;
        }
        // Includes HaircutType
       public async Task<bool> UpdateAsync(int appointmentId, int newBarberId, DateTime newAppointmentTime, HaircutType newHaircutType)
{
    var existingAppointment = await _repo.GetByIdAsync(appointmentId);
    if (existingAppointment == null)
        throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");

    // Conflict check...
    var allAppointments = await _repo.GetAllAsync();
    bool conflict = allAppointments.Any(a =>
        a.Id != appointmentId &&
        a.AppointmentDateAndTime == newAppointmentTime &&
        a.Barbers.Any(b => b.Id == newBarberId));

    if (conflict)
        throw new InvalidOperationException(
            $"Barber {newBarberId} is already booked at {newAppointmentTime}."
        );

    // Barber handling...
    var barber = existingAppointment.Barbers.FirstOrDefault(b => b.Id == newBarberId);
    if (barber == null)
    {
        var dbBarber = await _repo.GetBarberByIdAsync(newBarberId)
            ?? throw new KeyNotFoundException($"Barber with ID {newBarberId} not found.");
        existingAppointment.Barbers.Add(dbBarber);
    }

    // ✅ update both fields
    existingAppointment.AppointmentDateAndTime = newAppointmentTime;
    existingAppointment.HaircutType = newHaircutType;

    await _repo.SaveChangesAsync();
    return true;
} 
        // Upate an exiting appointment 
//        public async Task<bool> UpdateAsync(int appointmentId, int newBarberId, DateTime newAppointmentTime)
// {
//     // Get the appointment from repo
//     var existingAppointment = await _repo.GetByIdAsync(appointmentId);
//     if (existingAppointment == null)
//         throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");

//     // Simple in-memory conflict check
//     var allAppointments = await _repo.GetAllAsync();
//     bool conflict = allAppointments.Any(a =>
//         a.Id != appointmentId &&
//         a.AppointmentDateAndTime == newAppointmentTime &&
//         a.Barbers.Any(b => b.Id == newBarberId));

//     if (conflict)
//         throw new InvalidOperationException(
//             $"Barber {newBarberId} is already booked at {newAppointmentTime}."
//         );

//     // ✅ Load the barber from the DB instead of creating a new one
//     var barber = await _repo.GetBarberByIdAsync(newBarberId);
//     if (barber == null)
//         throw new KeyNotFoundException($"Barber with ID {newBarberId} not found.");

//     // Add barber if not already in the list
//     if (!existingAppointment.Barbers.Any(b => b.Id == newBarberId))
//     {
//         existingAppointment.Barbers.Add(barber);
//     }

//     // Update appointment time
//     existingAppointment.AppointmentDateAndTime = newAppointmentTime;

//     // Save changes
//     await _repo.SaveChangesAsync();
//     return true;
// }

        // Get all appointments containing a specific barber
    public async Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId)
        {
            var allAppointments = await _repo.GetAllAsync();
            return allAppointments
                .Where(a => a.Barbers.Any(b => b.Id == barberId))
                .ToList();
        }


    }
}