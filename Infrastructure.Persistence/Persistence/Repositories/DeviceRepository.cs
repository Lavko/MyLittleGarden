using Domain.Entities;
using Domain.Entities.Common;
using Domain.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence.Repositories;

public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
{
    private readonly AppDbContext _context;

    public DeviceRepository(AppDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IList<Device>> GetAllByTypeAsync(
        DeviceType type,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Devices.Where(s => s.Type == type).ToListAsync(cancellationToken);
    }
}
