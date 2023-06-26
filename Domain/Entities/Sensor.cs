using Domain.Entities.Common;

namespace Domain.Entities;

public class Sensor : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? MqttPath { get; set; } = null!;

    public int DeviceId { get; set; }
    public virtual Device Device { get; set; } = null!;

    public virtual IList<Measurement> Measurements { get; set; } = null!;
}
