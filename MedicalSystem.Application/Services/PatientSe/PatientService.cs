using MedicalSystem.Application.Interfaces.Repository.DoctorRe;
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Repository.Patients;
using MedicalSystem.Application.Interfaces.Services.Patients;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.ViewModel.Patient;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Services.PatientSe
{
    public class PatientService : Service<Patient>, IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IRepository<Clinic> _clinicRepository;
        private readonly IDoctorRepository _doctorRepository;
        public PatientService(IRepository<Patient> repository, IRepository<Clinic> clinicRepository, IDoctorRepository doctorRepository,IPatientRepository patientRepository) : base(repository)
        {
            _clinicRepository = clinicRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<bool> CreatePatientAsync(PatientViewModel model)
        {
            var patient = new Patient
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                IDCard = model.IDCard,
                Photo = model.Photo,
                DateOfBirth = model.DateOfBirth,
                Smorker = model.Smoker,
                HasAllergies = model.HasAllergies,
                ClinicId = model.ClinicId,
            };
            await _patientRepository.addAsync(patient);
            return true;
        }
      

        public async Task<bool> DeletePatientAsync(int id)
        {
      
            await _patientRepository.deleteAsync(id);
            return true;
        }

        public async Task<int> GetClinicByPatientIdAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIDAsync(patientId);
            if (patient == null) return 0;

            return patient.ClinicId;
        }

        public async Task<PatientViewModel?> GetPatientAsync(int id)
        {
            var patient = await _patientRepository.GetByIDAsync(id);
            if (patient == null) return null;

            return new PatientViewModel
            {
                Id = id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                IDCard = patient.IDCard,
                Photo = patient.Photo,
                DateOfBirth = patient.DateOfBirth,
                Smoker = patient.Smorker,
                HasAllergies = patient.HasAllergies,
                ClinicId = patient.ClinicId
            };
        }

        public async Task<IEnumerable<PatientViewModel>> GetPatientByClinicAsync(int clinicId)
        {
            var patients = await _patientRepository.GetAllAsync();
            return patients.Where(p => p.ClinicId == clinicId)
                .Select(p => new PatientViewModel
                {
                    Id= p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    Phone = p.Phone,
                    IDCard = p.IDCard,
                    Photo = p.Photo,
                    DateOfBirth = p.DateOfBirth,
                    Smoker = p.Smorker,
                    HasAllergies= p.HasAllergies,
                    ClinicId= p.ClinicId
                });
        }

        public async Task<string> GetPatientNameByIdAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIDAsync(patientId);
            return patient != null ? $"{patient.FirstName} {patient.LastName}" : string.Empty;
        }

        public async Task<bool> UpdatePatientAsync(PatientViewModel viewModel)
        {
            var patient = await _patientRepository.GetByIDAsync(viewModel.Id);
            if (patient == null) return false;

            patient.FirstName = viewModel.FirstName;
            patient.LastName = viewModel.LastName;
            patient.Email = viewModel.Email;
            patient.Phone = viewModel.Phone;
            patient.IDCard = viewModel.IDCard;
            patient.Photo = viewModel.Photo;
            patient.DateOfBirth = viewModel.DateOfBirth;
            patient.Smorker =viewModel.Smoker;
            patient.HasAllergies = viewModel.HasAllergies;
            patient.ClinicId = viewModel.ClinicId;

            await _patientRepository.updateAsync(patient);
            return true;
        }
    }
}
