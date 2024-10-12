using MedicalSystem.Application.Interfaces.Repository.AppointmentS;

using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence.Context;
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalSystem.Infrastructure.Persistence.Repositories.AppointmentRep
{
    public class AppointmentRepository : Repository<Appointment>,IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByClinicIdAsync(int clinicId)
        {
            return await _context.appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.ClinicId == clinicId)
                .ToListAsync();
        }
        public async Task<bool> AnyAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return await _context.Set<Appointment>().AnyAsync(predicate);
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            _context.appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                _context.appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
