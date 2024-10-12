using MedicalSystem.Domain.Common;
using MedicalSystem.Domain.Enums;


namespace MedicalSystem.Domain.Entities
{
    public class LabResult : BaseEntity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int LabTestId { get; set; }
        public LabTest LabTest { get; set; }
        public string? Result { get; set; }
        public Status Status { get; set; }
        public int ClinicId { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

    }
}
