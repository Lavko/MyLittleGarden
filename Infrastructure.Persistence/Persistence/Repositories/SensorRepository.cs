using Domain.Entities;
using Domain.Entities.Common;
using Domain.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence.Repositories;

public class SensorRepository : GenericRepository<Sensor>, ISensorRepository
{
    private readonly AppDbContext _context;

    public SensorRepository(AppDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IList<Sensor>> GetAllByDeviceAsync(
        int deviceId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Sensors
            .Where(s => s.DeviceId == deviceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<Sensor>> GetAllByTypeAsync(
        DeviceType type,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Sensors
            .Where(s => s.Device.Type == type)
            .ToListAsync(cancellationToken);
    }
}
