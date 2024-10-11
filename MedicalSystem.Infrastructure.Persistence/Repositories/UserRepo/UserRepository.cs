
using MedicalSystem.Application.Interfaces.Repository.UserRepository;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence.Context;
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Persistence.Repositories.UserRepo
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        
        public UserRepository(ApplicationDbContext context) : base(context)
        {
           
        }

        public async Task<IEnumerable<User>> GetUserByClinicAsync(int clinicId)
        {
            return await _context.Users.Where(u => u.ClinicID == clinicId).ToListAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
