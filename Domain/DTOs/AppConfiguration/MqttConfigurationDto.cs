namespace Domain.DTOs.AppConfiguration;

public class MqttConfigurationDto
{
    public int Id { get; set; }
    public bool IsClientEnabled { get; set; } = false;
    public string ClientId { get; set; } = null!;
    public string TcpServer { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}