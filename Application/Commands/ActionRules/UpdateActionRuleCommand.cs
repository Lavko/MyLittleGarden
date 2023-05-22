using Application.Scheduler;
using AutoMapper;
using Domain.DTOs.ActionRules;
using Domain.Entities;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.ActionRules;

public class UpdateActionRuleCommand : IRequest
{
    public UpdateActionRuleCommand(UpdateActionRuleDto updateActionRuleDto)
    {
        UpdateActionRuleDto = updateActionRuleDto;
    }

    public UpdateActionRuleDto UpdateActionRuleDto { get; }
}

public class UpdateActionRuleCommandHandler : IRequestHandler<UpdateActionRuleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISchedulerService _schedulerService;

    public UpdateActionRuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISchedulerService schedulerService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _schedulerService = schedulerService;
    }

    public async Task Handle(UpdateActionRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = await _unitOfWork.ActionRules.GetByIdAsync(request.UpdateActionRuleDto.Id, cancellationToken);

        if (rule is null)
        {
            throw new ArgumentException($"Rule with id: {request.UpdateActionRuleDto.Id}");
        }

        _mapper.Map(request.UpdateActionRuleDto, rule);

        
        if (request.UpdateActionRuleDto.IsSchedule 
            && request.UpdateActionRuleDto.Hour.HasValue
            && request.UpdateActionRuleDto.Minute.HasValue
            && request.UpdateActionRuleDto.Second.HasValue)
        {
            if (rule.ScheduledTime is null)
            {
                rule.ScheduledTime = new ScheduledTime();
            }

            rule.ScheduledTime.Hour = request.UpdateActionRuleDto.Hour.Value;
            rule.ScheduledTime.Minute = request.UpdateActionRuleDto.Minute.Value;
            rule.ScheduledTime.Second = request.UpdateActionRuleDto.Second.Value;

            await _schedulerService.RemoveJobFromScheduleAsync(rule.Id, cancellationToken);
            await _schedulerService.AddActionJobToScheduleAsync(rule.ScheduledTime, rule.Id, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}