using Domain.Entities.Common;

namespace Domain.DTOs.Sensors;

public class CreateSensorDto
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = null!;
    public string MqttPath { get; set; } = null!;
}