using System.Linq.Expressions;

namespace Domain.Repositories.Common;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default);
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IList<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    void Add(T entity);
    void AddRange(IList<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}