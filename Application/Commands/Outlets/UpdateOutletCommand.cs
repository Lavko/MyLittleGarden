using Domain.DTOs;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.Outlets;

public class UpdateOutletCommand : IRequest
{
    public UpdateOutletCommand(UpdateOutletDto updateOutletDto)
    {
        UpdateOutletDto = updateOutletDto;
    }

    public UpdateOutletDto UpdateOutletDto { get; }
}

public class UpdateOutletCommandHandler : IRequestHandler<UpdateOutletCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOutletCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateOutletCommand request, CancellationToken cancellationToken)
    {
        var outlet = await _unitOfWork.OutletConfigurations.GetByIdAsync(request.UpdateOutletDto.Id, cancellationToken);

        if (outlet is null)
        {
            throw new ArgumentException($"Outlet with id {request.UpdateOutletDto.Id} doesn't exists");
        }
        
        outlet.Name = request.UpdateOutletDto.Name;

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}