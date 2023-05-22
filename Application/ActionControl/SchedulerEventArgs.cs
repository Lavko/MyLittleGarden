using Domain.Entities;

namespace Application.ActionControl;

public class SchedulerEventArgs
{
    public ScheduledTime Time { get; set; } = null!;
    public int RuleId { get; set; }
}