namespace Domain.DTOs.ActionRules;

public class ActionRuleDto
{
    public int OutletConfigurationId { get; set; }
    public bool IsSchedule { get; set; }
    public int OutletId { get; set; }
    public string OutletAction { get; set; } = null!;
    public string MeasureProperty { get; set; } = null!;
    public string ComparisonType { get; set; } = null!;
    public string ComparisonValue { get; set; } = null!;
    public string Time { get; set; } = null!;
}