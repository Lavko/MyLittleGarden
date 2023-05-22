using Domain.Entities.Common;

namespace Domain.Entities;

public class EnvironmentMeasure : BaseEntity
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double GroundHumidity { get; set; }
    public double Pressure { get; set; }
}