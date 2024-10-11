
using MedicalSystem.Application.Interfaces.Repository.Auth;
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Services.Auth;
using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.ViewModel.User;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Application.Services.AuthService
{
    public class AuthService : Service<User>, IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IService<User> _userService;
        public AuthService(IAuthRepository userRepository, IService<User> userService, IRepository<User> repository) : base(repository)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        public async Task<User> login(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return user;
        }

        public async Task RegisterUser(RegisterViewModel registerViewModel)
        {
            if (await _userRepository.UsernameExistsAsync(registerViewModel.Username))
            {
                throw new Exception("El nombre de usuario ya existe.");
            }

            var newUser = new User
            {
                FirstName = registerViewModel.FirsName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.Email,
                Username = registerViewModel.Username,
                Password = HashPassword(registerViewModel.Password),
                ClinicID = registerViewModel.ClinicID,
                Role = registerViewModel.Role
            };

            await _userService.addAsync(newUser);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
