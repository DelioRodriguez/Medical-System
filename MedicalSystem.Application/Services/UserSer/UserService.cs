using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Repository.UserRepository;
using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.ViewModel.MantUser;
using MedicalSystem.Application.ViewModel.User;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Services.UserSer
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IService<User> _userService;

        public UserService(IUserRepository userRepository, IService<User> userService, IRepository<User> repository) : base(repository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsersByClinicAsync(int clinicId)
        {
            var users = await _userRepository.GetUserByClinicAsync(clinicId);
            return users.Select(user => new UserListViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.Username,
                Role = user.Role,
            });
        }
        public async Task<User> ValidateUserAsync(string username, string password)
        {

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }
        public async Task<int> GetClinicIdByUserIdAsync(int userId)
        {
            var user = await _userRepository.GetByIDAsync(userId);
            return user?.ClinicID ?? 0;
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            if (await UsernameExistsAsync(model.Username))
            {
                return false;
            }

            var newUser = new User
            {
                FirstName = model.FirsName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                Password = HashPassword(model.Password),
                ClinicID = model.ClinicID,
                Role = model.Role

            };
            await _userService.addAsync(newUser);
            return true;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<bool> UpdateUserAsync(EditUserViewModel model)
        {
            var existingUser = await _userRepository.GetByIDAsync(model.Id);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.FirstName = model.FirsName;
            existingUser.LastName = model.LastName;
            existingUser.Email = model.Email;
            existingUser.Username = model.Username;
            existingUser.Role = model.Role;

            if (!string.IsNullOrEmpty(model.Password))
            {
                existingUser.Password = HashPassword(model.Password);
            }


            await _userRepository.updateAsync(existingUser);
            return true;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var existingUser = await _userRepository.GetByIDAsync(userId);
            if (existingUser == null)
            {
                return false;
            }

            await _userService.DeleteAsync(userId);
            return true;
        }

        public async Task<UserListViewModel> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIDAsync(id);
            if (user == null) return null;

            return new UserListViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.Username,
                Role = user.Role,
            };
        }
        public async Task<bool> RegisterUserAsync(RegisterViewModel model, int loggedInUserId)
        {
            if (await UsernameExistsAsync(model.Username))
            {
                return false;
            }

            // Obtener la clínica del usuario logueado
            var clinicId = await GetClinicIdByUserIdAsync(loggedInUserId);

            var newUser = new User
            {
                FirstName = model.FirsName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                Password = HashPassword(model.Password),
                ClinicID = clinicId,
                Role = model.Role
            };

            await _userService.addAsync(newUser);
            return true;
        }


        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetByIDAsync(userId);
            if (user is null)
            {
                return false;
            }

            var hashedPassword = HashPassword(newPassword);

            user.Password = hashedPassword;

            await _userService.UpdateAsync(user);
            return true;
        }
    }
}
