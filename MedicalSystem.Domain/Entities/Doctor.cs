using MedicalSystem.Domain.Common;


namespace MedicalSystem.Domain.Entities
{
    public class Doctor : PersonEntity
    {
        public int ClinicID { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
