using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Repository.UserRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UsernameExistsAsync (string username);
        Task<IEnumerable<User>> GetUserByClinicAsync(int clinicID);
        Task<User> GetByUsernameAsync(string username);
    }
}
