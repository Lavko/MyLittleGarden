using Domain.Entities;
using Domain.Entities.Common;
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

    public SchedulerService(
        ILogger<SchedulerService> logger,
        ISchedulerFactory factory,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _scheduler = factory.GetScheduler().Result;
    }

    public async Task InitializeSavedJobs()
    {
        _logger.LogInformation("Initialization of schedule jobs from rules stored in DB started");

        var i2CDevices = await _unitOfWork.Devices.GetAllByTypeAsync(DeviceType.I2C);

        foreach (var i2CDevice in i2CDevices)
        {
            await AddI2CJobToScheduleAsync(i2CDevice);
        }
    }

    public async Task<List<string>> GetAllJobs(CancellationToken cancellationToken)
    {
        var jobKeys = await _scheduler.GetJobKeys(
            GroupMatcher<JobKey>.AnyGroup(),
            cancellationToken
        );

        return jobKeys.Select(key => key.Name).ToList();
    }

    public async Task AddI2CJobToScheduleAsync(
        Device device,
        CancellationToken cancellationToken = default
    )
    {
        var jobName = $"{nameof(I2CMeasureJob)}_{device.Id}";

        _logger.LogDebug("Creating new job for device id: {DeviceId}", device.Id);

        var job = JobBuilder
            .Create<I2CMeasureJob>()
            .WithIdentity(jobName)
            .UsingJobData("deviceId", device.Id)
            .Build();

        _logger.LogDebug("Creating new trigger for job with name: {JobName}", jobName);

        var trigger = TriggerBuilder
            .Create()
            .WithIdentity(jobName)
            .StartNow()
            .WithCronSchedule(GetCron(device))
            .Build();

        await _scheduler.ScheduleJob(job, trigger, cancellationToken);

        _logger.LogDebug("Job with name: {JobName} created", jobName);
    }

    public async Task RescheduleJob(Device device, CancellationToken cancellationToken = default)
    {
        var jobName = $"{nameof(I2CMeasureJob)}_{device.Id}";
        var trigger = TriggerBuilder
            .Create()
            .WithIdentity(jobName)
            .StartNow()
            .WithCronSchedule(GetCron(device))
            .Build();

        await _scheduler.RescheduleJob(new TriggerKey(jobName), trigger, cancellationToken);
    }

    public async Task RemoveDeviceJobFromScheduleAsync(
        int deviceId,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug("Removing job for device id: {DeviceId}", deviceId);
        await _scheduler.UnscheduleJob(
            new TriggerKey($"{nameof(I2CMeasureJob)}_{deviceId}"),
            cancellationToken
        );
    }

    private string GetCron(Device device)
    {
        var seconds =
            device.IntervalType == IntervalType.Seconds ? device.IntervalCount.ToString() : "*";
        var minutes =
            device.IntervalType == IntervalType.Minutes ? device.IntervalCount.ToString() : "*";
        var hours =
            device.IntervalType == IntervalType.Hours ? device.IntervalCount.ToString() : "*";
        var days = device.IntervalType == IntervalType.Days ? device.IntervalCount.ToString() : "*";

        return $"{seconds} {minutes} {hours} {days} * ?";
    }
}
