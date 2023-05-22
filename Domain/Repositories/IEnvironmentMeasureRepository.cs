using Domain.Entities;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IEnvironmentMeasureRepository : IGenericRepository<EnvironmentMeasure>
{
    Task<EnvironmentMeasure?> GetLastOrDefaultAsync(CancellationToken cancellationToken = default);
}