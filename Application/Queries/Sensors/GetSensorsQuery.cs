using System.Collections.Immutable;
using AutoMapper;
using Domain.DTOs.Sensors;
using Domain.Entities;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Sensors;

public class GetSensorsQuery : IRequest<IList<SensorDto>> { }

public class GetSensorsQueryHandler : IRequestHandler<GetSensorsQuery, IList<SensorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSensorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<SensorDto>> Handle(
        GetSensorsQuery request,
        CancellationToken cancellationToken
    )
    {
        var sensors = (List<Sensor>)await _unitOfWork.Sensors.GetAllAsync(cancellationToken);

        return _mapper.Map<IList<SensorDto>>(sensors.OrderBy(s => s.DeviceId));
    }
}
