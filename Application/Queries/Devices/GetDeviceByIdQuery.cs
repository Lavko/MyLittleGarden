using AutoMapper;
using Domain.DTOs.Devices;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Devices;

public class GetDeviceByIdQuery : IRequest<DeviceDto>
{
    public GetDeviceByIdQuery(int deviceId)
    {
        DeviceId = deviceId;
    }

    public int DeviceId { get; set; }
}

public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, DeviceDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDeviceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeviceDto> Handle(
        GetDeviceByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.DeviceId, cancellationToken);

        return _mapper.Map<DeviceDto>(device);
    }
}
