using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Persistence.Repositories.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task addAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task deleteAsync(int id)
        {
            var entity = await GetByIDAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIDAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task updateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
