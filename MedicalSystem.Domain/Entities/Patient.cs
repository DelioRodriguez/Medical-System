using MedicalSystem.Domain.Common;


namespace MedicalSystem.Domain.Entities
{
    public class Patient : PersonEntity
    {
        public DateTime DateOfBirth { get; set; }
        public bool Smorker { get; set; }
        public bool HasAllergies { get; set; }
        public int ClinicId { get; set; }

        public ICollection<LabResult> Results { get; set; } = new List<LabResult>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
