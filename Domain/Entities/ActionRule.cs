using Domain.Entities.Common;

namespace Domain.Entities;

public class ActionRule : BaseEntity
{
    public int OutletConfigurationId { get; set; }
    public bool IsSchedule { get; set; }
    public OutletConfiguration Outlet { get; set; } = null!;
    public OutletAction OutletAction { get; set; }
    public string MeasureProperty { get; set; } = null!;
    public ComparisonType ComparisonType { get; set; }
    public string ComparisonValue { get; set; } = null!;
    public int? ScheduledTimeId { get; set; }
    public ScheduledTime? ScheduledTime { get; set; } = null!;
}