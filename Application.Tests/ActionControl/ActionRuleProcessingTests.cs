using Application.ActionControl;
using Domain.Entities;
using Domain.Entities.Common;
using FluentAssertions;

namespace Application.Tests.ActionControl;

public class ActionRuleProcessingTests
{
    private readonly ActionRuleProcessor _actionRuleProcessor;

    public ActionRuleProcessingTests()
    {
        _actionRuleProcessor = new ActionRuleProcessor();
    }

    [Theory]
    [InlineData(nameof(EnvironmentMeasure.TimeStamp), "2022-10-01", ComparisonType.IsEmpty)]
    [InlineData(nameof(EnvironmentMeasure.TimeStamp), "2022-10-04", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.TimeStamp), "2022-10-03", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.TimeStamp), "2022-10-03", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.TimeStamp), "2022-10-02", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.TimeStamp), "2022-10-03", ComparisonType.IsEqual)]
    
    [InlineData(nameof(EnvironmentMeasure.Temperature), "22", ComparisonType.IsEmpty)]
    [InlineData(nameof(EnvironmentMeasure.Temperature), "22", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Temperature), "23", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Temperature), "22", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Temperature), "21", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Temperature), "22", ComparisonType.IsEqual)]
    
    [InlineData(nameof(EnvironmentMeasure.Humidity), "22", ComparisonType.IsEmpty)]
    [InlineData(nameof(EnvironmentMeasure.Humidity), "22", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Humidity), "23", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Humidity), "22", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Humidity), "21", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Humidity), "22", ComparisonType.IsEqual)]
    
    [InlineData(nameof(EnvironmentMeasure.Pressure), "1002", ComparisonType.IsEmpty)]
    [InlineData(nameof(EnvironmentMeasure.Pressure), "1002", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Pressure), "1003", ComparisonType.IsGreaterOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Pressure), "1002", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Pressure), "1001", ComparisonType.IsLesserOrEqual)]
    [InlineData(nameof(EnvironmentMeasure.Pressure), "1002", ComparisonType.IsEqual)]
    public void ProcessRule_ValidRule_ReturnsActionRuleProcessResult(string measureProperty, string comparisonValue, ComparisonType type)
    {
        // Arrange
        var measure = new EnvironmentMeasure
        {
            Id = 1,
            Humidity = type == ComparisonType.IsEmpty ? 0 : 22,
            Pressure = type == ComparisonType.IsEmpty ? 0 : 1002,
            Temperature = type == ComparisonType.IsEmpty ? 0 : 22,
            TimeStamp = type == ComparisonType.IsEmpty ? DateTime.MinValue : new DateTime(2022, 10, 3)
        };

        var rule = new ActionRule
        {
            Id = 11,
            ComparisonType = type,
            ComparisonValue = comparisonValue,
            MeasureProperty = measureProperty,
            Outlet = new OutletConfiguration
            {
                OutletId = 1,
                Id = 111
            },
            OutletAction = OutletAction.On
        };
        
        // Act
        var result = _actionRuleProcessor.ProcessRule(measure, rule);
        
        // Assert
        result.Should().NotBeNull();
        result!.RuleId.Should().Be(rule.Id);
        result!.MeasureId.Should().Be(measure.Id);
        result.Outlet.Should().NotBeNull();
        result.Outlet.Should().Be(rule.Outlet);
        result.Action.Should().Be(rule.OutletAction);
    }

    [Theory]
    [InlineData(nameof(EnvironmentMeasure.TimeStamp), "2022-1WRONG0-03")]
    [InlineData(nameof(EnvironmentMeasure.Temperature), "22WRONG")]
    public void ProcessRule_InvalidComparisonValue_ReturnsException(string measureProperty, string comparisonValue)
    {
        // Arrange
        var measure = new EnvironmentMeasure
        {
            Id = 1,
            Humidity = 22,
            Pressure = 1002,
            Temperature = 22,
            TimeStamp = new DateTime(2022, 10, 3)
        };

        var rule = new ActionRule
        {
            Id = 11,
            ComparisonType = ComparisonType.IsGreaterOrEqual,
            ComparisonValue = comparisonValue,
            MeasureProperty = measureProperty,
            Outlet = new OutletConfiguration
            {
                OutletId = 1,
                Id = 111
            },
            OutletAction = OutletAction.On
        };
        
        // Act & Assert
        Assert.Throws<InvalidCastException>(()=>_actionRuleProcessor.ProcessRule(measure, rule));
    }
}