using System.Device.I2c;
using Domain.Entities;
using Domain.Services;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RaspberryPi.Sensors;

public class Bme280Service : IEnvironmentMeasureService
{
    private readonly ILogger<Bme280Service> _logger;
    private readonly Bme280 _bme280;
    
    public Bme280Service(ILogger<Bme280Service> logger)
    {
        _logger = logger;

        var i2CSettings = new I2cConnectionSettings(1, Bmx280Base.SecondaryI2cAddress);
        var i2CDevice = I2cDevice.Create(i2CSettings);
        _bme280 = new Bme280(i2CDevice);
    }

    public EnvironmentMeasure GetMeasures()
    {
        _logger.LogInformation("Taking measures");

        try
        {
            var measurementTime = _bme280.GetMeasurementDuration();
            _bme280.SetPowerMode(Bmx280PowerMode.Forced);
            Thread.Sleep(measurementTime);

            _bme280.TryReadTemperature(out var tempValue);
            _bme280.TryReadPressure(out var preValue);
            _bme280.TryReadHumidity(out var humValue);

            _logger.LogInformation(
                "Measures taken at {Time}: temp: {Temp}, humidity: {Humidity}, pressure: {Pressure}",
                DateTime.Now.ToString("HH:mm:ss"),
                tempValue,
                humValue,
                preValue
            );
            return new EnvironmentMeasure
            {
                Temperature = tempValue.DegreesCelsius,
                Pressure = preValue.Hectopascals,
                Humidity = humValue.Percent,
                TimeStamp = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot take measures");
            throw;
        }
    }
}