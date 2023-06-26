using AutoMapper;
using Domain.DTOs.Sensors;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Sensors;

public class GetSensorsByDeviceIdQuery : IRequest<IList<SensorDto>>
{
    public GetSensorsByDeviceIdQuery(int deviceId)
    {
        DeviceId = deviceId;
    }

    public int DeviceId { get; set; }
}

public class GetSensorsByDeviceIdQueryHandler
    : IRequestHandler<GetSensorsByDeviceIdQuery, IList<SensorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSensorsByDeviceIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<SensorDto>> Handle(
        GetSensorsByDeviceIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var sensors = await _unitOfWork.Sensors.GetAllByDeviceAsync(
            request.DeviceId,
            cancellationToken
        );

        return _mapper.Map<IList<SensorDto>>(sensors);
    }
}
