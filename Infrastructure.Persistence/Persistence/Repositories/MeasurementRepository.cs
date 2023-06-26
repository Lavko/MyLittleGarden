using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Persistence.Repositories;

public class MeasurementRepository : GenericRepository<Measurement>, IMeasurementRepository
{
    private readonly AppDbContext _context;

    public MeasurementRepository(AppDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IList<Measurement>> GetAllBySensorAsync(
        int sensorId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Measurements
            .Where(s => s.SensorId == sensorId)
            .ToListAsync(cancellationToken);
    }
}
