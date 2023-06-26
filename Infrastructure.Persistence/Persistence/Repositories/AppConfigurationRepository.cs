using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence.Repositories;

public class AppConfigurationRepository : IAppConfigurationRepository
{
    private readonly AppDbContext _context;

    public AppConfigurationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppConfiguration?> GetCurrent(CancellationToken cancellationToken = default)
    {
        return await _context.AppConfigurations
            .Include(a => a.MqttConfiguration)
            .OrderBy(a => a.Id)
            .LastOrDefaultAsync(cancellationToken);
    }

    public void Add(AppConfiguration entity)
    {
        entity.TimeStamp = DateTime.Now;
        _context.AppConfigurations.Add(entity);
    }

    public void Remove(AppConfiguration entity)
    {
        _context.AppConfigurations.Remove(entity);
    }
}
