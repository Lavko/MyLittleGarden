using Application.Scheduler;
using AutoMapper;
using Domain.DTOs.Devices;
using Domain.Entities.Common;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Devices;

public class UpdateDeviceCommand : IRequest
{
    public UpdateDeviceCommand(UpdateDeviceDto dto)
    {
        Dto = dto;
    }

    public UpdateDeviceDto Dto { get; set; }
}

public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISchedulerService _schedulerService;

    public UpdateDeviceCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISchedulerService schedulerService
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _schedulerService = schedulerService;
    }

    public async Task Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var oldDevice = await _unitOfWork.Devices.GetByIdAsync(request.Dto.Id, cancellationToken);

        var isScheduleChange =
            oldDevice.Type == DeviceType.I2C
            && (
                oldDevice.IntervalCount != request.Dto.IntervalCount
                || oldDevice.IntervalType != request.Dto.IntervalType
            );

        _mapper.Map(request.Dto, oldDevice);

        if (isScheduleChange)
        {
            await _schedulerService.RescheduleJob(oldDevice, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
