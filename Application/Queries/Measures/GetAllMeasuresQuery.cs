using AutoMapper;
using Domain.DTOs.Measures;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Queries.Measures;

public class GetAllMeasuresQuery : IRequest<IList<MeasureDto>>
{
    
}

public class GetAllMeasuresQueryHandler : IRequestHandler<GetAllMeasuresQuery, IList<MeasureDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMeasuresQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<MeasureDto>> Handle(GetAllMeasuresQuery request, CancellationToken cancellationToken)
    {
        var measures = await _unitOfWork.Measures.GetAllAsync(cancellationToken);
        return _mapper.Map<IList<MeasureDto>>(measures);
    }
}