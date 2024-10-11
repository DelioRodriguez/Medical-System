using MedicalSystem.Domain.Common;

namespace MedicalSystem.Domain.Entities
{
    public class LabTest : BaseEntity
    {
        public string Name { get; set; }
        public int ClinicId { get; set; }
        public ICollection<LabResult> Results { get; set; } = new List<LabResult>();
    }
}
