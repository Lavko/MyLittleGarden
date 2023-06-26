using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Sensors;

public class DeleteSensorCommand : IRequest
{
    public DeleteSensorCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class DeleteSensorCommandHandler : IRequestHandler<DeleteSensorCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSensorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = await _unitOfWork.Sensors.GetByIdAsync(request.Id, cancellationToken);

        _unitOfWork.Sensors.Remove(sensor);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
