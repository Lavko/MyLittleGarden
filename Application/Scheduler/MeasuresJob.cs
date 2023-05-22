using Domain.Repositories.Common;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Application.Scheduler;

public class MeasuresJob : IJob
{
    
    private readonly IEnvironmentMeasureService _environmentMeasureService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MeasuresJob> _logger;

    public MeasuresJob(IEnvironmentMeasureService environmentMeasureService, IUnitOfWork unitOfWork, ILogger<MeasuresJob> logger)
    {
        _environmentMeasureService = environmentMeasureService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogDebug("Starting Measures scheduled task");
        
        var measures = _environmentMeasureService.GetMeasures();
        _unitOfWork.Measures.Add(measures);

        await _unitOfWork.CompleteAsync(context.CancellationToken);
    }
}