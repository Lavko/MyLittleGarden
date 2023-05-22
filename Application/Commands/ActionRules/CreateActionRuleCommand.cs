using Application.Scheduler;
using AutoMapper;
using Domain.DTOs.ActionRules;
using Domain.Entities;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.ActionRules;

public class CreateActionRuleCommand : IRequest
{
    public CreateActionRuleCommand(CreateActionRuleDto createActionRuleDto)
    {
        CreateActionRuleDto = createActionRuleDto;
    }

    public CreateActionRuleDto CreateActionRuleDto { get; }
}

public class CreateActionRuleCommandHandler : IRequestHandler<CreateActionRuleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISchedulerService _schedulerService;

    public CreateActionRuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISchedulerService schedulerService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _schedulerService = schedulerService;
    }
    
    public async Task Handle(CreateActionRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = _mapper.Map<ActionRule>(request.CreateActionRuleDto);
        _unitOfWork.ActionRules.Add(rule);
        
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        if (rule.IsSchedule)
        {
            await _schedulerService.AddActionJobToScheduleAsync(rule.ScheduledTime!, rule.Id, cancellationToken);
        }
    }
}