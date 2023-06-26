using Domain.Entities;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IMeasurementRepository : IGenericRepository<Measurement>
{
    Task<IList<Measurement>> GetAllBySensorAsync(
        int sensorId,
        CancellationToken cancellationToken = default
    );
}
