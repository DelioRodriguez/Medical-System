using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.MantUser;
using MedicalSystem.Application.ViewModel.User;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Interfaces.Services.UserS
{
    public interface IUserService : IService<User>
    {
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        Task<bool> UpdateUserAsync(EditUserViewModel model);
        Task<bool> UpdatePasswordAsync(int userId, string newPassword);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserListViewModel> GetByIdAsync(int id);
        Task<IEnumerable<UserListViewModel>> GetUsersByClinicAsync(int clinicId);
        Task<bool> UsernameExistsAsync(string username);
        Task<int> GetClinicIdByUserIdAsync(int userId);
        Task<User> ValidateUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(RegisterViewModel model, int loggedInUserId);

    }
}
