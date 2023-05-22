using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<EnvironmentMeasure> Measures { get; set; } = null!;
    public DbSet<ActionRule> ActionRules { get; set; } = null!;
    public DbSet<OutletConfiguration> OutletConfigurations { get; set; } = null!;
    public DbSet<TakenAction> TakenActions { get; set; } = null!;
}