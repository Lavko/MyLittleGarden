using Domain.Configurations;
using Domain.Entities;
using Domain.Repositories.Common;
using Microsoft.Extensions.Logging;

namespace Application.Initializers;

public class OutletInitializer
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly OutletMappingConfiguration _config;
    private readonly ILogger<OutletInitializer> _logger;

    public OutletInitializer(
        IUnitOfWork unitOfWork,
        OutletMappingConfiguration config,
        ILogger<OutletInitializer> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _config = config;
    }

    public async Task InitializeOutlets(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting initialization of outlet mappings");
        
        var configMappings = _config.Outlets;
        var savedMappings = await _unitOfWork.OutletConfigurations.GetAllAsync(cancellationToken);

        _logger.LogDebug("Removing obsolete outlet mappings");
        var outletIds = configMappings.Select(m => m.Key);
        
        _unitOfWork.OutletConfigurations
            .RemoveRange(savedMappings.Where(sm => !outletIds.Contains(sm.OutletId)));

        _logger.LogDebug("Updating outlet mappings");
        var mappingForSave = new List<OutletConfiguration>();
        foreach (var keyValuePair in configMappings)
        {
            var mapping = savedMappings.FirstOrDefault(sm => sm.OutletId == keyValuePair.Key);

            if (mapping is null)
            {
                mapping = new OutletConfiguration
                {
                    OutletId = keyValuePair.Key,
                    TimeStamp = DateTime.Now
                };
                mappingForSave.Add(mapping);
            }

            mapping.PinId = keyValuePair.Value;
        }
        
        _logger.LogDebug("Saving changes in outlet mappings");
        _unitOfWork.OutletConfigurations.AddRange(mappingForSave);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}