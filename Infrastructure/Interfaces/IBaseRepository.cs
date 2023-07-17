namespace PaelystSolution.Infrastructure.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> Create(T entity);
        Task<List<T>> AddRange(List<T> entity);
        bool Save();
    }
}
