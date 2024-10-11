using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Repository.Lab;
using MedicalSystem.Application.Interfaces.Repository.UserRepository;
using MedicalSystem.Application.Interfaces.Services.Lab;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.ViewModel.Lab;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Application.Services.Lab
{
    public class LabResulService : Service<LabResult>, ILabResultService
    {
        private readonly ILabResultRepository _resultRepository;
        private readonly IUserRepository _userRepository;
        public LabResulService(IRepository<LabResult> repository, ILabResultRepository labResultRepository, IUserRepository userRepository) : base(repository)
        {
            _resultRepository = labResultRepository;
            _userRepository = userRepository;
        }

        public async Task AddAsync(LabResult result)
        {
            await _resultRepository.addAsync(result);
        }

        public async Task<List<ResultLaboratoryListViewModel>> GetAllByOfficeIdAsync(int officeId)
        {
            var results = await _resultRepository.GetAllByOfficeIdAsync(officeId);
            return results.Select(r => new ResultLaboratoryListViewModel
            {
                Id = r.Id,
                PatientFullName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                PatientIdCard = r.Patient.IDCard,
                TestName = r.LabTest.Name,
                Status = r.Status
            }).ToList();
        }

        public async Task<List<ResultLaboratoryListViewModel>> GetAllPendingResultsAsync(int officeId)
        {
            var results = await _resultRepository.GetAllByOfficeIdAsync(officeId);
            return results
                .Where(r => r.ClinicId == officeId && r.Status == Status.PENDIENTEDERESULTADOS)
                .Select(r => new ResultLaboratoryListViewModel
                {
                    Id = r.Id,
                    PatientFullName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                    PatientIdCard = r.Patient.IDCard,
                    TestName = r.LabTest.Name,
                    Status = r.Status
                }).ToList();
        }

        public async Task CreateLabResultAsync(CreateResultViewModel model)
        {
            var labResult = new LabResult
            {
                PatientId = model.PatientId,
                LabTestId = model.LabTestId,
                Result = model.Result,
                Status = model.Status,
                ClinicId = model.ClinicId,
                AppointmentId = model.AppointmentId
            };

            await _resultRepository.addAsync(labResult);
          
        }
        public async Task<List<ResultLaboratoryListViewModel>> GetPendingResultsByPatientCardIdAsync(string patientCardId, int officeId)
        {
            var results = await _resultRepository.GetPendingResultByPatientIDCardAsync(patientCardId, officeId);
            return results
                .Where(r => r.Patient.ClinicId == officeId)
                .Select(r => new ResultLaboratoryListViewModel
                {
                    Id = r.Id,
                    PatientFullName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                    PatientIdCard = r.Patient.IDCard,
                    TestName = r.LabTest.Name,
                    Status = r.Status
                }).ToList();
        }

        public async Task<LabResult> GetResultByIdAsync(int resultId)
        {
            return await _resultRepository.GetResultByIdAsync(resultId);
        }

        public async Task<List<LabResult>> GetResultsByAppointmentIdAsync(int appointmentId)
        {
            return await _resultRepository.GetResultsByAppointmentIdAsync(appointmentId);
        }
        public async Task<int> GetClinicByResultIdAsync(int id)
        {
            var user = await _userRepository.GetByIDAsync(id);
            if (user is null) throw new Exception("Error");

            return user.ClinicID;
        }
        public async Task ReportResultAsync(ReportResultViewModel reportViewModel)
        {
            var result = await _resultRepository.GetResultByIdAsync(reportViewModel.ResultLaboratoryId);
            if (result != null)
            {
                result.Result = reportViewModel.Result;
                result.Status = Status.COMPLETADA;
                await _resultRepository.updateAsync(result);
            }
        }
    }
}
