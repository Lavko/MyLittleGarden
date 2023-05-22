using Domain.Entities.Common;

namespace Domain.DTOs.ActionRules;

public class CreateActionRuleDto
{
    public int OutletConfigurationId { get; set; }
    public bool IsSchedule { get; set; }
    public int OutletId { get; set; }
    public OutletAction OutletAction { get; set; }
    public string MeasureProperty { get; set; } = null!;
    public ComparisonType ComparisonType { get; set; }
    public string ComparisonValue { get; set; } = null!;
    public int? Hour { get; set; }
    public int? Minute { get; set; }
    public int? Second { get; set; }
}