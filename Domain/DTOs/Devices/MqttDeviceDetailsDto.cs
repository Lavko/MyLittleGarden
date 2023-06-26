namespace Domain.DTOs.Devices;

public class MqttDeviceDetailsDto
{
    public MqttDeviceDetailsDto(string chipName, IList<byte> i2CAddresses)
    {
        ChipName = chipName;
        I2CAddresses = i2CAddresses;
    }

    public string ChipName { get; set; }
    public IList<byte> I2CAddresses { get; set; }
}
