using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Application.Interfaces.Services.Generic;


namespace MedicalSystem.Application.Services.Generic
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task addAsync(T entity)
        {
            await _repository.addAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.deleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIDAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.updateAsync(entity);
        }
    }
}
