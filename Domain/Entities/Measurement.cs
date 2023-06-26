using Domain.Entities.Common;

namespace Domain.Entities;

public class Measurement : BaseEntity
{
    public string Result { get; set; } = null!;

    public int SensorId { get; set; }
    public virtual Sensor Sensor { get; set; } = null!;
}
