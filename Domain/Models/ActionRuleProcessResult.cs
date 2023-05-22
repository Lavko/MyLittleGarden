using Domain.Entities;
using Domain.Entities.Common;

namespace Domain.Models;

public class ActionRuleProcessResult
{
    public int RuleId { get; set; }
    public int MeasureId { get; set; }
    public OutletConfiguration Outlet { get; set; } = null!;
    public OutletAction Action { get; set; }
}