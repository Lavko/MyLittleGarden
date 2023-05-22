using AutoMapper;
using Domain.DTOs.TakenActions;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.TakenActions;

public class GetAllTakenActionsQuery : IRequest<IList<TakenActionDto>>
{
    
}

public class GetAllTakenActionsQueryHandler : IRequestHandler<GetAllTakenActionsQuery, IList<TakenActionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTakenActionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IList<TakenActionDto>> Handle(GetAllTakenActionsQuery request, CancellationToken cancellationToken)
    {
        var actions = await _unitOfWork.TakenActions.GetAllAsync(cancellationToken);
        return _mapper.Map<IList<TakenActionDto>>(actions);
    }
}