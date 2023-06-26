using Domain.Entities;
using Domain.Entities.Common;

namespace Domain.DTOs.Devices;

public class UpdateDeviceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;

    // I2C section
    public string ChipName { get; set; } = null!;
    public byte I2CAddress { get; set; }
    public int IntervalCount { get; set; }
    public IntervalType IntervalType { get; set; }
}
