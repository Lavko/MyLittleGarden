using AutoMapper;
using Domain.DTOs.ActionRules;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.ActionRules;

public class GetAllRulesQuery : IRequest<IList<ActionRuleDto>>
{
    
}

public class GetAllRulesQueryHandler : IRequestHandler<GetAllRulesQuery, IList<ActionRuleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllRulesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<ActionRuleDto>> Handle(GetAllRulesQuery request, CancellationToken cancellationToken)
    {
        var measures = await _unitOfWork.ActionRules.GetAllAsync(cancellationToken);
        return _mapper.Map<IList<ActionRuleDto>>(measures);
    }
}