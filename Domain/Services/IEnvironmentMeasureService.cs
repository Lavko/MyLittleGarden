using Domain.Entities;

namespace Domain.Services;

public interface IEnvironmentMeasureService
{
    EnvironmentMeasure GetMeasures();
}