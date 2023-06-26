using Domain.Entities;

namespace Domain.Services;

public interface ISensor
{
    string Name { get; }
    List<byte> I2CAddresses { get; }
    List<string> PossibleMeasurements { get; }
    Dictionary<string, string> TakeMeasures(byte i2CAddress);
}
