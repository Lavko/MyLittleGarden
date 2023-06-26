using Domain.Entities;
using Domain.Repositories.Common;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Application.Scheduler;

public class I2CMeasureJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;

    public I2CMeasureJob(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var deviceId = (int)context.MergedJobDataMap["deviceId"];
        var device = await _unitOfWork.Devices.GetByIdAsync(deviceId, context.CancellationToken);

        if (device is null)
        {
            throw new ArgumentException($"Device {deviceId} does not exists");
        }

        var sensor = _serviceProvider
            .GetServices<ISensor>()
            .FirstOrDefault(s => s.Name == device.ChipName);

        if (sensor is null)
        {
            throw new ArgumentException($"{device.Name} sensor does not exists in configuration");
        }

        var measurements = sensor.TakeMeasures(device.I2CAddress!.Value);

        var deviceSensors = await _unitOfWork.Sensors.GetAllByDeviceAsync(
            deviceId,
            context.CancellationToken
        );

        foreach (var keyValuePair in measurements)
        {
            var measurementSensor = deviceSensors.FirstOrDefault(ds => ds.Name == keyValuePair.Key);

            if (measurementSensor is not null)
            {
                _unitOfWork.Measurements.Add(
                    new Measurement
                    {
                        SensorId = measurementSensor.Id,
                        Result = keyValuePair.Value,
                        TimeStamp = DateTime.Now
                    }
                );
            }
        }

        await _unitOfWork.CompleteAsync();
    }
}
