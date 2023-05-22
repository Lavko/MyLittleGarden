namespace Domain.Repositories.Common;

public interface IUnitOfWork : IDisposable
{
    IEnvironmentMeasureRepository Measures { get; }
    IOutletConfigurationRepository OutletConfigurations { get; }
    IActionRuleRepository ActionRules { get; }
    ITakenActionRepository TakenActions { get; }
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}