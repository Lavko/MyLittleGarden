using Domain.Repositories.Common;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Application.Scheduler;

public class MeasuresJob : IJob
{
    
    private readonly IBme280Service _bme280Service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MeasuresJob> _logger;

    public MeasuresJob(IBme280Service bme280Service, IUnitOfWork unitOfWork, ILogger<MeasuresJob> logger)
    {
        _bme280Service = bme280Service;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogDebug("Starting Measures scheduled task");
        
        var measures = _bme280Service.GetMeasures();
        _unitOfWork.Measures.Add(measures);

        await _unitOfWork.CompleteAsync(context.CancellationToken);
    }
}