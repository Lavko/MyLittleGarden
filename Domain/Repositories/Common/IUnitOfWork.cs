namespace Domain.Repositories.Common;

public interface IUnitOfWork : IDisposable
{
    IDeviceRepository Devices { get; }
    ISensorRepository Sensors { get; }
    IMeasurementRepository Measurements { get; }
    IAppConfigurationRepository AppConfiguration { get; }
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
