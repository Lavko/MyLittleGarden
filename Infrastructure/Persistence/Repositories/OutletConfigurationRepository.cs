using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Repositories.Common;

namespace Infrastructure.Persistence.Repositories;

public class OutletConfigurationRepository : GenericRepository<OutletConfiguration>, IOutletConfigurationRepository
{
    public OutletConfigurationRepository(AppDbContext context) : base(context)
    {
    }
}