using AutoMapper;
using Domain.DTOs.AppConfiguration;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.AppConfiguration;

public class GetCurrentAppConfigurationQuery : IRequest<AppConfigurationDto> { }

public class GetCurrentAppConfigurationQueryHandler
    : IRequestHandler<GetCurrentAppConfigurationQuery, AppConfigurationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCurrentAppConfigurationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AppConfigurationDto> Handle(
        GetCurrentAppConfigurationQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _unitOfWork.AppConfiguration.GetCurrent(cancellationToken);

        return _mapper.Map<AppConfigurationDto>(result);
    }
}
