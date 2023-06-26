using Application.Queries.Measurements;
using Domain.DTOs.Measurements;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeasurementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/sensors/{id:int}/measurements")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<MeasurementDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBySensorId(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(new GetMeasurementsBySensorQuery(id), cancellationToken);
        return Ok(result);
    }
}
