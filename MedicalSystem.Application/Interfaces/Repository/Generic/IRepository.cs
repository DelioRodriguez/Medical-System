namespace MedicalSystem.Application.Interfaces.Repository.Generic
{
    public interface IRepository<T> where T : class
    {


        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIDAsync(int id);
        Task addAsync(T entity);
        Task updateAsync(T entity);
        Task deleteAsync(int id);
    }
}
