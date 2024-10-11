
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Repository.DoctorRe
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        IEnumerable<Doctor> GetDoctorsByClinic(int clinicId);
        Task<Doctor> GetByNameAsync(string username);
    }
}
