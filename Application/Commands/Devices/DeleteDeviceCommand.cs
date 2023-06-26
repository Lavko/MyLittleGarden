using Application.Scheduler;
using Domain.Entities.Common;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Devices;

public class DeleteDeviceCommand : IRequest
{
    public DeleteDeviceCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulerService _schedulerService;

    public DeleteDeviceCommandHandler(IUnitOfWork unitOfWork, ISchedulerService schedulerService)
    {
        _unitOfWork = unitOfWork;
        _schedulerService = schedulerService;
    }

    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id, cancellationToken);

        _unitOfWork.Devices.Remove(device);
        
        if (device.Type == DeviceType.I2C)
        {
            await _schedulerService.RemoveDeviceJobFromScheduleAsync(device.Id, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
