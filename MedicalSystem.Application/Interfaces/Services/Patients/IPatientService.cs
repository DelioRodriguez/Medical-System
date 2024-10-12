using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.Patient;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Services.Patients
{
    public interface IPatientService : IService<Patient>
    {
        Task<bool> CreatePatientAsync(PatientViewModel model);
        Task<bool> UpdatePatientAsync(PatientViewModel viewModel);
        Task<bool> DeletePatientAsync(int id);
        Task<PatientViewModel> GetPatientAsync(int id);

        Task<IEnumerable<PatientViewModel>> GetPatientByClinicAsync(int clinicId);
        Task<string> GetPatientNameByIdAsync(int patientId);
        Task<int> GetClinicByPatientIdAsync(int patientId);
    }
}
