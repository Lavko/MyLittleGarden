using Domain.Entities;
using Domain.Entities.Common;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface ISensorRepository : IGenericRepository<Sensor>
{
    Task<IList<Sensor>> GetAllByDeviceAsync(
        int deviceId,
        CancellationToken cancellationToken = default
    );
    Task<IList<Sensor>> GetAllByTypeAsync(
        DeviceType type,
        CancellationToken cancellationToken = default
    );
}
