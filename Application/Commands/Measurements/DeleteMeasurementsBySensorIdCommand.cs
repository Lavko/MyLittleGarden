using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Measurements;

public class DeleteMeasurementsBySensorIdCommand : IRequest
{
    public DeleteMeasurementsBySensorIdCommand(int sensorId)
    {
        SensorId = sensorId;
    }

    public int SensorId { get; set; }
}

public class DeleteMeasurementsBySensorIdCommandHandler
    : IRequestHandler<DeleteMeasurementsBySensorIdCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMeasurementsBySensorIdCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteMeasurementsBySensorIdCommand request,
        CancellationToken cancellationToken
    )
    {
        var measurements = await _unitOfWork.Measurements.GetAllBySensorAsync(
            request.SensorId,
            cancellationToken
        );
        _unitOfWork.Measurements.RemoveRange(measurements);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
