using AutoMapper;
using Domain.DTOs.Sensors;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Sensors;

public class UpdateSensorCommand : IRequest
{
    public UpdateSensorCommand(UpdateSensorDto dto)
    {
        Dto = dto;
    }

    public UpdateSensorDto Dto { get; set; }
}

public class UpdateSensorCommandHandler : IRequestHandler<UpdateSensorCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSensorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateSensorCommand request, CancellationToken cancellationToken)
    {
        var oldSensor = await _unitOfWork.Sensors.GetByIdAsync(request.Dto.Id, cancellationToken);
        _mapper.Map(request.Dto, oldSensor);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
