using Domain.Repositories;
using Domain.Repositories.Common;

namespace Infrastructure.Persistence.Persistence.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Devices = new DeviceRepository(context);
        Sensors = new SensorRepository(context);
        Measurements = new MeasurementRepository(context);
        AppConfiguration = new AppConfigurationRepository(context);
    }

    public IDeviceRepository Devices { get; set; }
    public ISensorRepository Sensors { get; set; }
    public IMeasurementRepository Measurements { get; set; }
    public IAppConfigurationRepository AppConfiguration { get; set; }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
