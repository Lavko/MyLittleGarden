using Application.Scheduler;
using AutoMapper;
using Domain.Repositories.Common;
using MediatR;

namespace Application.Commands.ActionRules;

public class DeleteActionRuleCommand : IRequest
{
    public DeleteActionRuleCommand(int id)
    {
        Id = id;
    }

    public int Id { get; }
}

public class DeleteActionRuleCommandHandler : IRequestHandler<DeleteActionRuleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulerService _schedulerService;

    public DeleteActionRuleCommandHandler(IUnitOfWork unitOfWork, ISchedulerService schedulerService)
    {
        _unitOfWork = unitOfWork;
        _schedulerService = schedulerService;
    }

    public async Task Handle(DeleteActionRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = await _unitOfWork.ActionRules.GetByIdAsync(request.Id, cancellationToken);

        if (rule is null)
        {
            throw new ArgumentException($"Rule with id: {request.Id} does not exist");
        }
        
        _unitOfWork.ActionRules.Remove(rule);

        if (rule.IsSchedule)
        {
            await _schedulerService.RemoveJobFromScheduleAsync(rule.Id, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}