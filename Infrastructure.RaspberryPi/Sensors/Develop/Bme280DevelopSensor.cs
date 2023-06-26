using Domain.Entities;
using Domain.Services;
using Iot.Device.Bmxx80;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RaspberryPi.Sensors.Develop;

public class Bme280DevelopSensor : ISensor
{
    private readonly ILogger<Bme280DevelopSensor> _logger;

    public Bme280DevelopSensor(ILogger<Bme280DevelopSensor> logger)
    {
        _logger = logger;
    }

    public string Name => "BME280";

    public List<byte> I2CAddresses =>
        new() { Bmx280Base.DefaultI2cAddress, Bmx280Base.SecondaryI2cAddress };

    public List<string> PossibleMeasurements => new() { "Temperature", "Humidity", "Pressure" };

    public Dictionary<string, string> TakeMeasures(byte i2CAddress)
    {
        _logger.LogInformation("Taking measures");

        var measurements = new Dictionary<string, string>();

        measurements.Add("Temperature", new Random().Next(0, 42).ToString());
        measurements.Add("Humidity", new Random().Next(10, 80).ToString());
        measurements.Add("Pressure", new Random().Next(980, 1080).ToString());

        return measurements;
    }
}
