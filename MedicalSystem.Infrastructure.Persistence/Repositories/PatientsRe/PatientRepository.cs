
using MedicalSystem.Application.Interfaces.Repository.Patients;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence.Context;
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Persistence.Repositories.PatientsRe
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Patient> GetByNameAsync(string username)
        {
            return await _context.Patients.FirstOrDefaultAsync(u => u.FirstName == username);
        }

        public  IEnumerable<Patient> GetPatientByClinic(int clinicId)
        {
            return  _context.Patients.Where(u => u.ClinicId == clinicId).ToList();
        }
    }
}
