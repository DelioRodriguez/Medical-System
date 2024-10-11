using MedicalSystem.Domain.Common;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Cause { get; set; }
        public Status Status { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public ICollection<LabTest> LabTests { get; set; } = new List<LabTest>();
        public int ClinicId { get; set; }
  
      
    }
}
