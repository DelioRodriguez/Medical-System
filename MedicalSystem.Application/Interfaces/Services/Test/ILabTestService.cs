using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.LabTestS;
using MedicalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Interfaces.Services.Test
{
    public interface ILabTestService : IService<LabTest>
    {
        Task<bool> CreateLabTestAsync(LabTestViewModel model);
        Task<bool> UpdateLabTestAsync(LabTestViewModel model);
        Task<bool> DeleteLabTestAsync(int id);
        Task<LabTestViewModel> GetLabTestAsync(int id);
        Task<IEnumerable<LabTestViewModel>> GetLabTestsByClinicAsync(int appointmentId);
    }
}
