using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.Ammoin;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;


namespace MedicalSystem.Application.Interfaces.Services.Appoint
{
    public interface IAppointmentService : IService<Appointment>
    {
        Task<IEnumerable<AppointmentViewModel>> GetAppointmentsAsync(int clinicId);
        Task<bool> DoctorHasAppointmentsAsync(int doctorId);
        Task<bool> PatientHasAppointmentsAsync(int patientId);
        Task<AppointmentViewModel> CreateAppointmentAsync(AppointmentViewModel model);
        Task DeleteAppointmentAsync(int id);
        Task UpdateAppointmentStatusToPendingResultsAsync(int appointmentId);

    }
}
