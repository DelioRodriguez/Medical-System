using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Interfaces.Repository.Lab
{
    public interface ILabResultRepository : IRepository<LabResult>
    {
        Task<List<LabResult>> GetPendingResultByPatientIDCardAsync(string patientID, int clinicID);
        Task<LabResult> GetResultByIdAsync(int resultId);
        Task<List<LabResult>> GetAllByOfficeIdAsync(int officeId);
        Task<List<LabResult>> GetResultsByAppointmentIdAsync(int appointmentId);
        Task<IEnumerable<LabResult>> GetCompletedLabResultsByAppointmentIdAsync(int appointmentId);
        Task<List<LabResult>> GetByAppointmentIdAsync(int appointmentId);
    }
}
