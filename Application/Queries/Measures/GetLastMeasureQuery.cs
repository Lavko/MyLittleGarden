using AutoMapper;
using Domain.DTOs.Measures;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Measures;

public class GetLastMeasureQuery : IRequest<MeasureDto>
{
    
}

public class GetLastMeasureQueryHandler : IRequestHandler<GetLastMeasureQuery, MeasureDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLastMeasureQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<MeasureDto> Handle(GetLastMeasureQuery request, CancellationToken cancellationToken)
    {
        var measure = await _unitOfWork.Measures.GetLastOrDefaultAsync(cancellationToken);
        return _mapper.Map<MeasureDto>(measure);
    }
}