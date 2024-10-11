using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Repository.Test;
using MedicalSystem.Application.Interfaces.Services.Test;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.ViewModel.LabTestS;
using MedicalSystem.Domain.Entities;


namespace MedicalSystem.Application.Services.Test
{
    public class LabTestService : Service<LabTest>, ILabTestService
    {
        private readonly ILabTestRepository _testRepository;
        public LabTestService(IRepository<LabTest> repository, ILabTestRepository labTestRepository) : base(repository)
        {
            _testRepository = labTestRepository;
        }

        public async Task<bool> CreateLabTestAsync(LabTestViewModel model)
        {
            var labTest = new LabTest
            {
                Name = model.Name,
                ClinicId = model.ClinicId
            };
             await _testRepository.addAsync(labTest);
            return true;

        }

        public async Task<bool> DeleteLabTestAsync(int id)
        {
           
            await _testRepository.deleteAsync(id);
            return true;
        }

        public async Task<LabTestViewModel> GetLabTestAsync(int id)
        {
            var labTest = await _testRepository.GetByIDAsync(id);
            if (labTest == null) return null;

            return new LabTestViewModel
            {
                Id = labTest.Id,
                Name = labTest.Name,
                ClinicId = labTest.ClinicId
            };
        }

        public async Task<IEnumerable<LabTestViewModel>> GetLabTestsByClinicAsync(int appointmentId)
        {
            var labTest = await _testRepository.GetAllAsync();
            return labTest.Where(p => p.ClinicId == appointmentId)
                .Select(p => new LabTestViewModel
                {
                    Id=p.Id,
                    Name = p.Name,
                    ClinicId = p.ClinicId
                });
            
        }

        public async Task<bool> UpdateLabTestAsync(LabTestViewModel model)
        {
            var LabTest = await _testRepository.GetByIDAsync(model.Id);
            if (LabTest == null) return false;

            LabTest.Name = model.Name;
            LabTest.ClinicId = model.ClinicId;

            await _testRepository.updateAsync(LabTest);
            return true;
        }
    }
}
