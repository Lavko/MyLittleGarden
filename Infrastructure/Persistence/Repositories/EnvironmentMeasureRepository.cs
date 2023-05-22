using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EnvironmentMeasureRepository : GenericRepository<EnvironmentMeasure>, IEnvironmentMeasureRepository
{
    private readonly AppDbContext _context;
    public EnvironmentMeasureRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public new async Task<IList<EnvironmentMeasure>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Measures.OrderBy(m => m.Id).ToListAsync(cancellationToken);
    }

    public async Task<EnvironmentMeasure?> GetLastOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Measures.OrderBy(m => m.Id).LastOrDefaultAsync(cancellationToken);
    }
}