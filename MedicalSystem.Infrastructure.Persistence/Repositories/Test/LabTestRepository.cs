
using MedicalSystem.Application.Interfaces.Repository.Test;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence.Context;
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;

namespace MedicalSystem.Infrastructure.Persistence.Repositories.Test
{
    public class LabTestRepository : Repository<LabTest>, ILabTestRepository
    {
        public LabTestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<LabTest> GetlabTestByClinicID(int clinicID)
        {
            return _context.labTests.Where(d => d.ClinicId == clinicID);
        }
      
    }
}
