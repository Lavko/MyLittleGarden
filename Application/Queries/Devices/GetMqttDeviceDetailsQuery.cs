using Domain.DTOs.Devices;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Queries.Devices;

public class GetMqttDeviceDetailsQuery : IRequest<List<MqttDeviceDetailsDto>> { }

public class GetMqttDeviceDetailsQueryHandler
    : IRequestHandler<GetMqttDeviceDetailsQuery, List<MqttDeviceDetailsDto>>
{
    private readonly IServiceProvider _serviceProvider;

    public GetMqttDeviceDetailsQueryHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<List<MqttDeviceDetailsDto>> Handle(
        GetMqttDeviceDetailsQuery request,
        CancellationToken cancellationToken
    )
    {
        var sensorDevice = _serviceProvider.GetServices<ISensor>().ToList();

        if (sensorDevice is null)
        {
            throw new ArgumentException($"Sensors does not exists in configuration");
        }

        var details = sensorDevice
            .Select(x => new MqttDeviceDetailsDto(x.Name, x.I2CAddresses))
            .ToList();
        return Task.FromResult(details);
    }
}
