namespace Domain.Services;

public interface IActionControlService
{
    Task TakeActionsAsync(CancellationToken cancellationToken = default);
    Task TakeActionByRuleId(int ruleId);
}