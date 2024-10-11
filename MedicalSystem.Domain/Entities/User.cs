using MedicalSystem.Domain.Common;

namespace MedicalSystem.Domain.Entities
{
    public class User : BaseEntity
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int ClinicID { get; set; }
        public Clinic Clinic { get; set; }


    }
}
