using AutoMapper;
using Domain.DTOs.Devices;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Devices;

public class GetDevicesQuery : IRequest<IList<DeviceDto>> { }

public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, IList<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDevicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<DeviceDto>> Handle(
        GetDevicesQuery request,
        CancellationToken cancellationToken
    )
    {
        var devices = await _unitOfWork.Devices.GetAllAsync(cancellationToken);

        return _mapper.Map<IList<DeviceDto>>(devices);
    }
}
