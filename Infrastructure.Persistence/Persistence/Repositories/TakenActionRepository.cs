using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence.Repositories;

public class TakenActionRepository : GenericRepository<TakenAction>, ITakenActionRepository
{
    private readonly AppDbContext _context;
    public TakenActionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public new async Task<IList<TakenAction>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TakenActions
            .Include(ta => ta.ActionRule)
            .Include(ta => ta.EnvironmentMeasure)
            .ToListAsync(cancellationToken);
    }
}