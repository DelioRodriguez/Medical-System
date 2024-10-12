using MedicalSystem.Application.Interfaces.Repository.AppointmentS;
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Services.Appoint;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.ViewModel.Ammoin;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;


namespace MedicalSystem.Application.Services.AppointmentSer
{
    public class AppointmentService : Service<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IRepository<Appointment> repository, IAppointmentRepository appointmentRepository) : base(repository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAppointmentsAsync(int clinicId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByClinicIdAsync(clinicId);
            return appointments.Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                Date = a.Date,
                Time = a.Time,
                Cause = a.Cause,
                Status = a.Status,
                DoctorName = a.Doctor.FirstName,
                PatientName = a.Patient.FirstName
            });
        }
        public async Task<bool> DoctorHasAppointmentsAsync(int doctorId)
        {
            return await _appointmentRepository.AnyAsync(a => a.DoctorId == doctorId);
        }


        public async Task<AppointmentViewModel> CreateAppointmentAsync(AppointmentViewModel model)
        {
            var appointment = new Appointment
            {
                Date = model.Date,
                Time = model.Time,
                Cause = model.Cause,
                Status = Status.PENDIENTEDECONDSULTA, 
                DoctorId = model.DoctorId,
                PatientId = model.PatientId,
                ClinicId = model.ClinicId
            };

            await _appointmentRepository.CreateAppointmentAsync(appointment);
            return model;
        }
        public async Task UpdateAppointmentStatusToPendingResultsAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIDAsync(appointmentId);
            if (appointment != null)
            {
                appointment.Status = Status.PENDIENTEDERESULTADOS; 
                await _appointmentRepository.updateAsync(appointment);
            }
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            await _appointmentRepository.DeleteAppointmentAsync(id);
        }

        public async Task<bool> PatientHasAppointmentsAsync(int patientId)
        {
            return await _appointmentRepository.AnyAsync(a => a.PatientId == patientId);
        }
    }
}
