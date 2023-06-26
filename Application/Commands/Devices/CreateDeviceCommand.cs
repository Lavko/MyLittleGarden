using Application.Scheduler;
using AutoMapper;
using Domain.DTOs.Devices;
using Domain.Entities;
using Domain.Entities.Common;
using Domain.Repositories.Common;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Commands.Devices;

public class CreateDeviceCommand : IRequest
{
    public CreateDeviceCommand(CreateDeviceDto dto)
    {
        Dto = dto;
    }

    public CreateDeviceDto Dto { get; set; }
}

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISchedulerService _schedulerService;
    private readonly IServiceProvider _serviceProvider;

    public CreateDeviceCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISchedulerService schedulerService,
        IServiceProvider serviceProvider
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _schedulerService = schedulerService;
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = _mapper.Map<Device>(request.Dto);

        if (device.Type == DeviceType.I2C)
        {
            var sensorDevice = _serviceProvider
                .GetServices<ISensor>()
                .FirstOrDefault(s => s.Name == device.ChipName);

            if (sensorDevice is null)
            {
                throw new ArgumentException(
                    $"{device.Name} sensor does not exists in configuration"
                );
            }

            sensorDevice.PossibleMeasurements.ForEach(
                m => device.Sensors.Add(new Sensor { Name = m })
            );

            _unitOfWork.Devices.Add(device);
            await _unitOfWork.CompleteAsync(cancellationToken);

            await _schedulerService.AddI2CJobToScheduleAsync(device, cancellationToken);
        }
        else
        {
            _unitOfWork.Devices.Add(device);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
