using Domain.Entities;
using Domain.Entities.Common;
using Domain.Models;

namespace Application.ActionControl;

public class ActionRuleProcessor : IActionRuleProcessor
{
    public ActionRuleProcessResult? ProcessRule(EnvironmentMeasure measure, ActionRule rule)
    {
        var valid = false;
        switch (rule.MeasureProperty)
        {
            case nameof(measure.TimeStamp):
                valid = ValidateRule(measure.TimeStamp, rule);
                break;
            case nameof(measure.GroundHumidity):
                break;
            case nameof(measure.Humidity):
                valid = ValidateRule(measure.Humidity, rule);
                break;
            case nameof(measure.Temperature):
                valid = ValidateRule(measure.Temperature, rule);
                break;
            case nameof(measure.Pressure):
                valid = ValidateRule(measure.Pressure, rule);
                break;
            default:
                throw new ArgumentException($"Unknown measure property: {rule.MeasureProperty}");
        }

        if (valid)
        {
            return new ActionRuleProcessResult
            {
                RuleId = rule.Id,
                MeasureId = measure.Id,
                Outlet = rule.Outlet,
                Action = rule.OutletAction
            };
        }

        return null;
    }

    private static bool ValidateRule(double value, ActionRule rule)
    {
        return rule.ComparisonType switch
        {
            ComparisonType.IsEmpty => value == 0,
            ComparisonType.IsEqual => value.Equals(CastComparisonValueToDouble(rule.ComparisonValue)),
            ComparisonType.IsGreaterOrEqual => value <= CastComparisonValueToDouble(rule.ComparisonValue),
            ComparisonType.IsLesserOrEqual => value >= CastComparisonValueToDouble(rule.ComparisonValue),
            _ => throw new ArgumentOutOfRangeException(nameof(rule.ComparisonType))
        };
    }
    
    private static bool ValidateRule(DateTime value, ActionRule rule)
    {
        return rule.ComparisonType switch
        {
            ComparisonType.IsEmpty => value == DateTime.MinValue,
            ComparisonType.IsEqual => value.Equals(CastComparisonValueToDateTime(rule.ComparisonValue)),
            ComparisonType.IsGreaterOrEqual => value <= CastComparisonValueToDateTime(rule.ComparisonValue),
            ComparisonType.IsLesserOrEqual => value >= CastComparisonValueToDateTime(rule.ComparisonValue),
            _ => throw new ArgumentOutOfRangeException(nameof(rule.ComparisonType))
        };
    }

    private static double CastComparisonValueToDouble(string comparisonValue)
    {
        var tryParse = double.TryParse(comparisonValue, out var doubleValue);
        if (!tryParse)
        {
            throw new InvalidCastException($"Cannot cast \"{comparisonValue}\" to double");
        }

        return doubleValue;
    }
    
    private static DateTime CastComparisonValueToDateTime(string comparisonValue)
    {
        var tryParse = DateTime.TryParse(comparisonValue, out var outValue);
        if (!tryParse)
        {
            throw new InvalidCastException($"Cannot cast \"{comparisonValue}\" to DateTime");
        }

        return outValue;
    }
}