using Application.Queries.TakenActions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class TakenActionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TakenActionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTakenActionsQuery(), cancellationToken);
        return Ok(result);
    }
}