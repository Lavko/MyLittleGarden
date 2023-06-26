using AutoMapper;
using Domain.DTOs.Sensors;
using Domain.Entities;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Sensors;

public class CreateSensorCommand : IRequest
{
    public CreateSensorCommand(CreateSensorDto dto)
    {
        Dto = dto;
    }

    public CreateSensorDto Dto { get; set; }
}

public class CreateSensorCommandHandler : IRequestHandler<CreateSensorCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSensorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateSensorCommand request, CancellationToken cancellationToken)
    {
        var sensor = _mapper.Map<Sensor>(request.Dto);

        _unitOfWork.Sensors.Add(sensor);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
