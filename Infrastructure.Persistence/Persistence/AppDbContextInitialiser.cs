using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Persistence;

public class AppDbContextInitialiser
{
    private readonly ILogger<AppDbContextInitialiser> _logger;
    private readonly AppDbContext _context;

    public AppDbContextInitialiser(ILogger<AppDbContextInitialiser> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                _logger.LogDebug("Migrating DB");
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }
    
    public async Task SeedAsync()
    {
        try
        {
            _logger.LogDebug("Starting seed DB task");
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private Task TrySeedAsync()
    {
        return Task.CompletedTask;
    }
}