using Domain.Repositories;
using Domain.Repositories.Common;

namespace Infrastructure.Persistence.Persistence.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Measures = new EnvironmentMeasureRepository(context);
        OutletConfigurations = new OutletConfigurationRepository(context);
        ActionRules = new ActionRuleRepository(context);
        TakenActions = new TakenActionRepository(context);
    }
    
    public IEnvironmentMeasureRepository Measures { get; set; }
    public IOutletConfigurationRepository OutletConfigurations { get; set; }
    public IActionRuleRepository ActionRules { get; set; }
    public ITakenActionRepository TakenActions { get; set; }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}