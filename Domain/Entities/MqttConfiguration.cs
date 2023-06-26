using Domain.Entities.Common;

namespace Domain.Entities;

public class MqttConfiguration : BaseEntity
{
    public int AppConfigurationId { get; set; }
    public bool IsClientEnabled { get; set; } = false;
    public string ClientId { get; set; } = null!;
    public string TcpServer { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
