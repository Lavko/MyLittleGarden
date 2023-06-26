using AutoMapper;
using Domain.DTOs.Measurements;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Measurements;

public class GetMeasurementsBySensorQuery : IRequest<IList<MeasurementDto>>
{
    public GetMeasurementsBySensorQuery(int sensorId)
    {
        SensorId = sensorId;
    }

    public int SensorId { get; set; }
}

public class GetMeasurementsBySensorQueryHandler
    : IRequestHandler<GetMeasurementsBySensorQuery, IList<MeasurementDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMeasurementsBySensorQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<MeasurementDto>> Handle(
        GetMeasurementsBySensorQuery request,
        CancellationToken cancellationToken
    )
    {
        var measurements = await _unitOfWork.Measurements.GetAllBySensorAsync(
            request.SensorId,
            cancellationToken
        );

        return _mapper.Map<IList<MeasurementDto>>(measurements);
    }
}
