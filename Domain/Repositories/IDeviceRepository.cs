using Domain.Entities;
using Domain.Entities.Common;
using Domain.Repositories.Common;

namespace Domain.Repositories;

public interface IDeviceRepository : IGenericRepository<Device>
{
    Task<IList<Device>> GetAllByTypeAsync(
        DeviceType type,
        CancellationToken cancellationToken = default
    );
}
