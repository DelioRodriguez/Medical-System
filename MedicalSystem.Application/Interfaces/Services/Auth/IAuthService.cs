using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.User;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Services.Auth
{
    public interface IAuthService : IService<User>
    {
        Task RegisterUser(RegisterViewModel registerViewModel);
        Task<User> login(string username, string password);
    }
}
