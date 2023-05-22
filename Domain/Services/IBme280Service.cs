using Domain.Entities;

namespace Domain.Services;

public interface IBme280Service
{
    EnvironmentMeasure GetMeasures();
}