using Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Application.Scheduler;

public class ScheduledActionJob : IJob
{
    private readonly IActionControlService _actionControlService;
    private readonly ILogger<ScheduledActionJob> _logger;

    public ScheduledActionJob(IActionControlService actionControlService, ILogger<ScheduledActionJob> logger)
    {
        _actionControlService = actionControlService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var ruleId = (int)context.MergedJobDataMap["ruleId"];
        
        _logger.LogDebug("Starting job for rule id: {RuleId}", ruleId);
        
        await _actionControlService.TakeActionByRuleId(ruleId);
    }
}