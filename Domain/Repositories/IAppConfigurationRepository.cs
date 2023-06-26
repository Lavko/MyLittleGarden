using Domain.Entities;

namespace Domain.Repositories;

public interface IAppConfigurationRepository
{
    Task<AppConfiguration?> GetCurrent(CancellationToken cancellationToken = default);
    void Add(AppConfiguration entity);
    void Remove(AppConfiguration entity);
}
