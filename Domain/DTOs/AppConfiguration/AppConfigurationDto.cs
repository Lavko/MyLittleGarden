namespace Domain.DTOs.AppConfiguration;

public class AppConfigurationDto
{
    public int Id { get; set; }
    public MqttConfigurationDto MqttConfiguration { get; set; } = null!;
}