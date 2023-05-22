using Domain.Entities;
using Domain.Repositories.Common;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl.Matchers;

namespace Application.Scheduler;

public class SchedulerService : ISchedulerService
{
    private readonly IScheduler _scheduler;
    private readonly ILogger<SchedulerService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public SchedulerService(ILogger<SchedulerService> logger, ISchedulerFactory factory, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _scheduler = factory.GetScheduler().Result;
    }

    public async Task InitializeSavedJobs()
    {
        _logger.LogInformation("Initialization of schedule jobs from rules stored in DB started");
        var rules = await _unitOfWork.ActionRules.GetAllSchedulesAsync();
        
        foreach (var rule in rules)
        {
            if (rule.ScheduledTime is not null)
            {
                await AddActionJobToScheduleAsync(rule.ScheduledTime, rule.Id);
            }
        }
        
        _logger.LogInformation("Initialized {JobsCount} jobs from DB", rules.Count);
    }

    public async Task AddActionJobToScheduleAsync(
        ScheduledTime time,
        int ruleId,
        CancellationToken cancellationToken = default)
    {
        var jobName = $"{nameof(ScheduledActionJob)}_{ruleId}";
        
        _logger.LogDebug("Creating new job for rule id: {RuleId}", ruleId);
        
        var job = JobBuilder.Create<ScheduledActionJob>()
            .WithIdentity(jobName)
            .UsingJobData("ruleId", ruleId)
            .Build();

        _logger.LogDebug("Creating new trigger for job with name: {JobName}", jobName);

        var cron = $"0 {time.Minute} {time.Hour} * * ?";
        var trigger = TriggerBuilder.Create()
            .WithIdentity(jobName)
            .StartNow()
            .WithCronSchedule(cron)
            .Build();
        
        await _scheduler.ScheduleJob(job, trigger, cancellationToken);
        
        _logger.LogDebug("Job with name: {JobName} created", jobName);
    }

    public async Task RemoveJobFromScheduleAsync(int ruleId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Removing job for rule id: {RuleId}", ruleId);
        await _scheduler.UnscheduleJob(new TriggerKey($"{nameof(ScheduledActionJob)}_{ruleId}"), cancellationToken);
    }

    public async Task<List<string>> GetAllJobs(CancellationToken cancellationToken)
    {
        var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);

        return jobKeys.Select(key => key.Name).ToList();
    }
}