using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;

namespace Infrastructure.Persistence.Persistence.Repositories;

public class OutletConfigurationRepository : GenericRepository<OutletConfiguration>, IOutletConfigurationRepository
{
    public OutletConfigurationRepository(AppDbContext context) : base(context)
    {
    }
}