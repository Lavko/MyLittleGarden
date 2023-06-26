using Domain.Entities.Common;

namespace Domain.Entities;

public class Device : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Path { get; set; }
    public DeviceType Type { get; set; }

    // I2C section
    public string? ChipName { get; set; }
    public byte? I2CAddress { get; set; }
    public int? IntervalCount { get; set; }
    public IntervalType? IntervalType { get; set; }
    public virtual IList<Sensor> Sensors { get; set; } = new List<Sensor>();
}
