
using MedicalSystem.Application.Interfaces.Repository.DoctorRe;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence.Context;
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Persistence.Repositories.DoctorRepo
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Doctor> GetByNameAsync(string username)
        {
            return await _context.Doctors.FirstOrDefaultAsync(u => u.FirstName == username);
        }

        public IEnumerable<Doctor> GetDoctorsByClinic(int clinicId)
        {
           return _context.Doctors.Where(d => d.ClinicID == clinicId).ToList();
        }
    }
}
