using Domain.Entities.Common;

namespace Domain.Entities;

public class AppConfiguration : BaseEntity
{
    public MqttConfiguration MqttConfiguration { get; set; } = null!;
}
