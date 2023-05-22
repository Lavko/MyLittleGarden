using Domain.Entities;
using Domain.Entities.Common;
using Domain.Models;
using Domain.Repositories.Common;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.ActionControl;

public class ActionControlService : IActionControlService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActionRuleProcessor _actionRuleProcessor;
    private readonly IOutletService _outletService;
    private readonly ILogger<ActionControlService> _logger;

    public ActionControlService(IUnitOfWork unitOfWork, IActionRuleProcessor actionRuleProcessor, IOutletService outletService, ILogger<ActionControlService> logger)
    {
        _unitOfWork = unitOfWork;
        _actionRuleProcessor = actionRuleProcessor;
        _outletService = outletService;
        _logger = logger;
    }

    public async Task TakeActionsAsync(CancellationToken cancellationToken = default)
    {
        var rules = await _unitOfWork.ActionRules.GetAllAsync(cancellationToken);

        if (rules.Count > 0)
        {
            var lastMeasure = await _unitOfWork.Measures.GetLastOrDefaultAsync(cancellationToken);

            var results = GetValidRules(rules, lastMeasure);
            _logger.LogDebug("Found {ResultCount} valid action rules for processing", results.Count);

            await ProcessResultsAsync(results, cancellationToken);
        }
    }
    
    public async Task TakeActionByRuleId(int ruleId)
    {
        _logger.LogDebug("Process rule");
        var rule = await _unitOfWork.ActionRules.GetByIdAsync(ruleId);

        if (rule is not null)
        {
            var result = new ActionRuleProcessResult
            {
                Action = rule.OutletAction,
                MeasureId = -1,
                Outlet = rule.Outlet,
                RuleId = rule.Id
            };
            await ProcessResultsAsync(new List<ActionRuleProcessResult> {result});
        }
    }

    private List<ActionRuleProcessResult> GetValidRules(
        IEnumerable<ActionRule> rules,
        EnvironmentMeasure? lastMeasure)
    {
        if (lastMeasure is null)
        {
            _logger.LogError("Cannot find any measures");
            throw new ArgumentNullException(nameof(lastMeasure));
        }

        return rules.Select(actionRule => _actionRuleProcessor.ProcessRule(lastMeasure, actionRule)).Where(result => result is not null).ToList()!;
    }

    private async Task ProcessResultsAsync(List<ActionRuleProcessResult> results, CancellationToken cancellationToken = default)
    {
        foreach (var result in results)
        {
            var isOutletOn = _outletService.IsOutletOn(result.Outlet.PinId);
            if (result.Action == OutletAction.On)
            {
                if (isOutletOn)
                {
                    _logger.LogDebug("Nothing to do. {OutletName} is already on", result.Outlet.Name);
                }
                else
                {
                    _logger.LogDebug("Turning on {OutletName}", result.Outlet.Name);
                    _outletService.TurnOutletOn(result.Outlet.PinId);
                    var takenAction = new TakenAction(result.RuleId, result.MeasureId);
                    _unitOfWork.TakenActions.Add(takenAction);
                }
            }
            else
            {
                if (!isOutletOn)
                {
                    _logger.LogDebug("Nothing to do. {OutletName} is already off", result.Outlet.Name);
                }
                else
                {
                    _logger.LogDebug("Turning off {OutletName}", result.Outlet.Name);
                    _outletService.TurnOutletOff(result.Outlet.PinId);
                    var takenAction = new TakenAction(result.RuleId, result.MeasureId);
                    _unitOfWork.TakenActions.Add(takenAction);
                }
            }
        }
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}