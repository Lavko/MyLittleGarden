using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Sensor> Sensors { get; set; } = null!;
    public DbSet<Measurement> Measurements { get; set; } = null!;
    public DbSet<AppConfiguration> AppConfigurations { get; set; } = null!;
}
