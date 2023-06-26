using AutoMapper;
using Domain.DTOs.Measurements;
using Domain.Entities;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Measurements;

public class CreateMeasurementCommand : IRequest
{
    public CreateMeasurementCommand(CreateMeasurementDto dto)
    {
        Dto = dto;
    }

    public CreateMeasurementDto Dto { get; set; }
}

public class CreateMeasurementCommandHandler : IRequestHandler<CreateMeasurementCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMeasurementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateMeasurementCommand request, CancellationToken cancellationToken)
    {
        var measurement = _mapper.Map<Measurement>(request.Dto);

        _unitOfWork.Measurements.Add(measurement);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
