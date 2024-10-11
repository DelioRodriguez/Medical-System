using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Repository.Auth
{
    public interface IAuthRepository : IRepository<User>
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
    }
}
