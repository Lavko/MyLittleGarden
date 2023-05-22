using Domain.Entities.Common;

namespace Domain.Entities;

public class TakenAction : BaseEntity
{
    public TakenAction()
    {
    }

    public TakenAction(int actionRuleId, int environmentMeasureId)
    {
        ActionRuleId = actionRuleId;
        EnvironmentMeasureId = environmentMeasureId;
    }

    public int ActionRuleId { get; set; }
    public virtual ActionRule ActionRule { get; set; } = null!;
    public int EnvironmentMeasureId { get; set; }
    public virtual EnvironmentMeasure EnvironmentMeasure { get; set; } = null!;
}