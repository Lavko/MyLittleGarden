using Domain.Entities;
using Domain.Models;

namespace Application.ActionControl;

public interface IActionRuleProcessor
{
    ActionRuleProcessResult? ProcessRule(EnvironmentMeasure measure, ActionRule rule);
}