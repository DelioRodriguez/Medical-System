using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.Doctor;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Services.DoctorSe
{
    public interface IDoctorService : IService<Doctor>
    {
        Task<bool> CreateDoctorAsync(DoctorViewModel model);
        Task<bool> UpdateDoctorAsync(DoctorViewModel viewModel);
        Task<bool> DeleteDoctorAsync(int id);
        Task<DoctorViewModel> GetDoctorAsync(int id);
        Task<IEnumerable<DoctorViewModel>> GetAllDoctorsAsync();
        Task<IEnumerable<DoctorViewModel>> GetDoctorByClinicAsync(int id);
        Task<int> GetClinicByDoctorIdAsync(int id);
        Task<string> GetDoctorNameByIdAsync(int doctorId);
    }
}
