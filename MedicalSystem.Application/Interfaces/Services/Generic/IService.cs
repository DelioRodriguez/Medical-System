namespace MedicalSystem.Application.Interfaces.Services.Generic
{
    public interface IService<T> where T : class
    {
        Task addAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
