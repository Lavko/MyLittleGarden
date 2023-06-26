using System.Device.I2c;
using Domain.Entities;
using Domain.Services;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RaspberryPi.Sensors;

public class Bme280Sensor : ISensor
{
    private readonly ILogger<Bme280Sensor> _logger;

    public Bme280Sensor(ILogger<Bme280Sensor> logger)
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

        var i2CSettings = new I2cConnectionSettings(1, i2CAddress);
        var i2CDevice = I2cDevice.Create(i2CSettings);
        var bme280 = new Bme280(i2CDevice);

        try
        {
            var measurementTime = bme280.GetMeasurementDuration();
            bme280.SetPowerMode(Bmx280PowerMode.Forced);
            Thread.Sleep(measurementTime);

            bme280.TryReadTemperature(out var tempValue);
            bme280.TryReadPressure(out var preValue);
            bme280.TryReadHumidity(out var humValue);

            measurements.Add("Temperature", tempValue.ToString());
            measurements.Add("Pressure", preValue.ToString());
            measurements.Add("Humidity", humValue.ToString());

            _logger.LogInformation(
                "Measures taken at {Time}: temp: {Temp}, humidity: {Humidity}, pressure: {Pressure}",
                DateTime.Now.ToString("HH:mm:ss"),
                tempValue,
                humValue,
                preValue
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot take measures");
            throw;
        }

        return measurements;
    }
}
