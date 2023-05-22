namespace Domain.DTOs.TakenActions;

public class TakenActionDto
{
    public int ActionRuleId { get; set; }
    public int EnvironmentMeasureId { get; set; }
    public int OutletId { get; set; }
    public string Action { get; set; } = null!;
    public string MeasureProperty { get; set; } = null!;
    public string ComparisonValue { get; set; } = null!;
    public string ComparisonType { get; set; } = null!;
    public string MeasureValue { get; set; } = null!;
}