using AutoMapper;
using Domain.DTOs.Sensors;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Sensors;

public class GetSensorByIdQuery : IRequest<SensorDto>
{
    public GetSensorByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

public class GetSensorByIdQueryHandler : IRequestHandler<GetSensorByIdQuery, SensorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSensorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SensorDto> Handle(
        GetSensorByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var sensor = await _unitOfWork.Sensors.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<SensorDto>(sensor);
    }
}
