namespace Lavadoras.Application.IRepositories
{
    public interface IBaseRepository<T>
    {
        Task<T> Get(int id);
        Task<T> Create(T instance);
        Task<T> Update(T instance);
        Task<List<T>> GetAll();
    }
}
