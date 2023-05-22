using Application.Queries.Measures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnvironmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllMeasuresQuery(), cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("/last")]
    public async Task<IActionResult> GetLast(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLastMeasureQuery(), cancellationToken);

        return Ok(result);
    }
}