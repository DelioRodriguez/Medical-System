using MedicalSystem.Application.Interfaces.Repository.Lab;
using MedicalSystem.Application.Interfaces.Repository.Test;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Infrastructure.Persistence.Context;
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Persistence.Repositories.Lab
{
    public class LabResultRepository : Repository<LabResult>, ILabResultRepository
    {


        public LabResultRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<LabResult>> GetAllByOfficeIdAsync(int officeId)
        {
            return await _context.labResults
                .Include(r => r.Patient)
                .Include(r => r.LabTest)
                .Where(r => r.ClinicId == officeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<LabResult>> GetCompletedLabResultsByAppointmentIdAsync(int appointmentId)
        {
            return await _context.labResults
                .Where(result => result.AppointmentId == appointmentId && result.Status == Status.COMPLETADA)
                .ToListAsync();
        }

        public async Task<List<LabResult>> GetPendingResultByPatientIDCardAsync(string patientID, int clinicID)
        {
            return await _context.labResults
                .Include(r => r.Patient)
                .Include(r => r.LabTest)
                .Where(r => r.Patient.IDCard == patientID && r.Status == Status.PENDIENTEDERESULTADOS && r.ClinicId == clinicID)
                .ToListAsync();
        }
        public async Task<List<LabResult>> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.labResults
                .Include(lr => lr.LabTest) 
                .Where(lr => lr.AppointmentId == appointmentId)
                .ToListAsync();
        }


        public async Task<LabResult> GetResultByIdAsync(int resultId)
        {
            return await _context.labResults
                .Include(r=> r.Patient)
                .Include(r => r.LabTest)
                .FirstOrDefaultAsync( r=> r.Id == resultId );
        }

        public async Task<List<LabResult>> GetResultsByAppointmentIdAsync(int appointmentId)
        {
            return await _context.labResults
                .Include(r => r.Patient)
                .Include(r => r.LabTest)
                .ToListAsync();
        }
    }
}
