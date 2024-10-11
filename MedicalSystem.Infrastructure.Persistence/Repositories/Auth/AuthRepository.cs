using Microsoft.EntityFrameworkCore;
using MedicalSystem.Infrastructure.Persistence.Context; // Asegúrate de usar el namespace correcto
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;
using MedicalSystem.Application.Interfaces.Repository.Auth;
using MedicalSystem.Domain.Entities;

public class AuthRepository : Repository<User>, IAuthRepository
{
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}
