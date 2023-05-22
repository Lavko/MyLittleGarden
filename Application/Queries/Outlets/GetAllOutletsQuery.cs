using AutoMapper;
using Domain.DTOs;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Outlets;

public class GetAllOutletsQuery : IRequest<IList<OutletDto>>
{
    
}

public class GetAllOutletsQueryHandler : IRequestHandler<GetAllOutletsQuery, IList<OutletDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllOutletsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<OutletDto>> Handle(GetAllOutletsQuery request, CancellationToken cancellationToken)
    {
        var outletMappings = await _unitOfWork.OutletConfigurations.GetAllAsync(cancellationToken);
        return _mapper.Map<IList<OutletDto>>(outletMappings);
    }
}