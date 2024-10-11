
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Repository.AppointmentS
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByClinicIdAsync(int clinicId);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task DeleteAppointmentAsync(int id);
    }
}
