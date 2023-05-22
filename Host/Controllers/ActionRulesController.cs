using Application.Commands.ActionRules;
using Application.Queries.ActionRules;
using Domain.DTOs.ActionRules;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ActionRulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActionRulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllRulesQuery(), cancellationToken);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateActionRuleDto request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateActionRuleCommand(request), cancellationToken);

        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]UpdateActionRuleDto request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateActionRuleCommand(request), cancellationToken);

        return Ok();
    }
    
    [HttpDelete("/{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteActionRuleCommand(id), cancellationToken);

        return Ok();
    }
}