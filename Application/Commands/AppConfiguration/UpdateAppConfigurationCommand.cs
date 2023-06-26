using AutoMapper;
using Domain.DTOs.AppConfiguration;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.AppConfiguration;

public class UpdateAppConfigurationCommand : IRequest
{
    public UpdateAppConfigurationCommand(AppConfigurationDto dto)
    {
        Dto = dto;
    }

    public AppConfigurationDto Dto { get; set; }
}

public class UpdateAppConfigurationCommandHandler : IRequestHandler<UpdateAppConfigurationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAppConfigurationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateAppConfigurationCommand request,
        CancellationToken cancellationToken
    )
    {
        var configuration = await _unitOfWork.AppConfiguration.GetCurrent(cancellationToken);

        _mapper.Map(request.Dto, configuration);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
