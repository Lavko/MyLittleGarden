using System.Linq.Expressions;
using Domain.Entities.Common;
using Domain.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence.Repositories.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    
    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public void Add(T entity)
    {
        entity.TimeStamp = DateTime.Now;
        _context.Set<T>().Add(entity);
    }
    
    public void AddRange(IList<T> entities)
    {
        foreach (var baseEntity in entities)
        {
            baseEntity.TimeStamp = DateTime.Now;
        }

        _context.Set<T>().AddRange(entities);
    }
    
    public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().Where(expression).ToListAsync(cancellationToken);
    }
    
    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }
    
    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
    }
    
    public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
    }
    
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}