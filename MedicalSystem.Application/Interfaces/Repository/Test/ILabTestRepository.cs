using MedicalSystem.Application.Interfaces.Repository.Generic;

using LabTest = MedicalSystem.Domain.Entities.LabTest;

namespace MedicalSystem.Application.Interfaces.Repository.Test
{
    public interface ILabTestRepository : IRepository<LabTest>
    {
        IEnumerable<LabTest> GetlabTestByClinicID(int clinicID);
       

    }
}
