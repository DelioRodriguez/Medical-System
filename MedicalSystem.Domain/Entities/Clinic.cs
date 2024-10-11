using MedicalSystem.Domain.Common;

namespace MedicalSystem.Domain.Entities
{
    public class Clinic : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
