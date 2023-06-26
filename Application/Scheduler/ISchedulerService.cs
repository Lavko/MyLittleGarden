using Domain.Entities;

namespace Application.Scheduler;

public interface ISchedulerService
{
    Task InitializeSavedJobs();

    Task AddI2CJobToScheduleAsync(Device device, CancellationToken cancellationToken = default);

    Task RescheduleJob(Device device, CancellationToken cancellationToken = default);

    Task RemoveDeviceJobFromScheduleAsync(int deviceId, CancellationToken cancellationToken);
}
