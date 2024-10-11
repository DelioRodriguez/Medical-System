
using MedicalSystem.Application.Interfaces.Repository.DoctorRe;
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Repository.UserRepository;
using MedicalSystem.Application.Interfaces.Services.DoctorSe;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.ViewModel.Doctor;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Services.DoctorSer
{
    public class DoctorServices :Service<Doctor>, IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public DoctorServices(IDoctorRepository doctorRepository, IUserService userService , IUserRepository user, IRepository<Doctor> repository) : base(repository)
        {
            this._doctorRepository = doctorRepository;
            this._userService = userService;
            this._userRepository = user;
        }
        public async Task<bool> CreateDoctorAsync(DoctorViewModel model)
        {
            if( await _userService.UsernameExistsAsync(model.Firstname))
            {
                return false;
            }

            var doctor = new Doctor
            {
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Email = model.Email,
                Phone = model.Phone,
                IDCard = model.IDCard,
                Photo = model.Photo,
                ClinicID = model.ClinicID
            };
            await _doctorRepository.addAsync(doctor);
            return true;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIDAsync(id);
            if(doctor is null) return false;

            await _doctorRepository.deleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<DoctorViewModel>> GetAllDoctorsAsync()
        {
          
            var doctors = await _doctorRepository.GetAllAsync();

           
            var doctorViewModels = doctors.Select(d => new DoctorViewModel
            {
                Id = d.Id,
                Firstname = d.FirstName,
                Lastname = d.LastName,
                Email = d.Email,
                Phone = d.Phone,
                IDCard = d.IDCard,
                Photo = d.Photo,
                ClinicID = d.ClinicID
            });

            return doctorViewModels;
        }

        public async Task<int> GetClinicByDoctorIdAsync(int id)
        {
            var user = await _userRepository.GetByIDAsync(id);
            if (user is null) throw new Exception("Error");

            return user.ClinicID;
        }

        public async Task<DoctorViewModel> GetDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIDAsync(id);
            if (doctor is null) return null;

            var doctorViewModel = new DoctorViewModel
            {
                Id = doctor.Id,
                Firstname = doctor.FirstName,
                Lastname = doctor.LastName,
                Email = doctor.Email,
                Phone = doctor.Phone,
                IDCard = doctor.IDCard,
                Photo = doctor.Photo,
                ClinicID = doctor.ClinicID
            };
            return doctorViewModel;
        }

        public async Task<IEnumerable<DoctorViewModel>> GetDoctorByClinicAsync(int id)
        {
            var doctors =  _doctorRepository.GetDoctorsByClinic(id);
            return doctors.Select( doctor => new DoctorViewModel
            {
                Id = doctor.Id,
                Firstname = doctor.FirstName,
                Lastname = doctor.LastName,
                Email = doctor.Email,
                IDCard = doctor.IDCard,
                Photo = doctor.Photo,
                Phone = doctor.Phone,
                
            });
        }

        public async Task<string> GetDoctorNameByIdAsync(int doctorId)
        {
            var doctor = await _doctorRepository.GetByIDAsync(doctorId);
            return doctor != null? $"{doctor.FirstName} {doctor.LastName}" : string.Empty;
        }

        public async Task<bool> UpdateDoctorAsync(DoctorViewModel viewModel)
        {
            var doctor = await _doctorRepository.GetByIDAsync(viewModel.Id);
            if(doctor is null) return false;

            doctor.FirstName = viewModel.Firstname;
            doctor.LastName = viewModel.Lastname;
            doctor.Email = viewModel.Email;
            doctor.Phone = viewModel.Phone;
            doctor.IDCard = viewModel.IDCard;
            doctor.Photo = viewModel.Photo;

            await _doctorRepository.updateAsync(doctor);
            return true;
        }
    }
}
