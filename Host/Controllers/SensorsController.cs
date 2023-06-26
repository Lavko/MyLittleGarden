using Application.Commands.Sensors;
using Application.Queries.Sensors;
using Domain.DTOs.Sensors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class SensorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SensorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(new GetSensorByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("/devices/{id:int}/sensors")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByDeviceId(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(new GetSensorsByDeviceIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<SensorDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSensorsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateSensorDto dto,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new CreateSensorCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] UpdateSensorDto dto,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new UpdateSensorCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteSensorCommand(id), cancellationToken);
        return Ok();
    }
}
