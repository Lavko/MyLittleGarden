using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence.Repositories;

public class ActionRuleRepository : GenericRepository<ActionRule>, IActionRuleRepository
{
    private readonly AppDbContext _context;
    public ActionRuleRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public new async Task<IList<ActionRule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActionRules
            .Include(ta => ta.Outlet)
            .Include(ta => ta.ScheduledTime)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IList<ActionRule>> GetAllSchedulesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActionRules
            .Include(ta => ta.Outlet)
            .Include(ta => ta.ScheduledTime)
            .Where(ta => ta.IsSchedule)
            .ToListAsync(cancellationToken);
    }

    public new async Task<ActionRule?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ActionRules.Include(ar => ar.Outlet).Include(ar => ar.ScheduledTime).FirstOrDefaultAsync(ar => ar.Id == id, cancellationToken: cancellationToken);
    }
}