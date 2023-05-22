using Domain.Entities;

namespace Application.Scheduler;

public interface ISchedulerService
{
    Task InitializeSavedJobs();
    Task AddActionJobToScheduleAsync(ScheduledTime time, int ruleId, CancellationToken cancellationToken);
    Task RemoveJobFromScheduleAsync(int ruleId, CancellationToken cancellationToken);
    Task<List<string>> GetAllJobs(CancellationToken cancellationToken);
}