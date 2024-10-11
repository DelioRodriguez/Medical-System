using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Interfaces.Repository.Patients
{
    public interface IPatientRepository : IRepository<Patient>
    {
        IEnumerable<Patient> GetPatientByClinic(int clinicId);
        Task<Patient> GetByNameAsync(string username);
    }
}
