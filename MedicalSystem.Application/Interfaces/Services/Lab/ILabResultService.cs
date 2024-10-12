using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.Extras;
using MedicalSystem.Application.ViewModel.Lab;
using MedicalSystem.Domain.Entities;


namespace MedicalSystem.Application.Interfaces.Services.Lab
{
    public interface ILabResultService : IService<LabResult>
    {
        Task AddAsync(LabResult result);
        Task<List<ResultLaboratoryListViewModel>> GetAllByOfficeIdAsync(int officeId);
        Task<List<ResultLaboratoryListViewModel>> GetAllPendingResultsAsync(int officeId);
        Task<List<ResultLaboratoryListViewModel>> GetPendingResultsByPatientCardIdAsync(string patientCardId, int officeId);
        Task<LabResult> GetResultByIdAsync(int resultId);
        Task<int> GetClinicByResultIdAsync(int id);
        Task<bool> LabtestHasResults(int labTest);
        Task<List<LabResult>> GetResultsByAppointmentIdAsync(int appointmentId);
        Task ReportResultAsync(ReportResultViewModel reportViewModel);
        Task CreateLabResultAsync(CreateResultViewModel model);

        Task<List<LabResult>> GetAllConfirmedResultsByAppointmentIdAsync(int officeId, int appointmentId);
    }
}
