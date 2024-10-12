
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;
using System.Linq.Expressions;

namespace MedicalSystem.Application.Interfaces.Repository.AppointmentS
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByClinicIdAsync(int clinicId);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task<bool> AnyAsync(Expression<Func<Appointment, bool>> predicate);
        Task DeleteAppointmentAsync(int id);
    }
}
