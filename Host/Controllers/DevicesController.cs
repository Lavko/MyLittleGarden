using Application.Commands.Devices;
using Application.Queries.Devices;
using Domain.DTOs.Devices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeviceDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(new GetDeviceByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<DeviceDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDevicesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("mqttDeviceDetails")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<DeviceDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetChipNames(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMqttDeviceDetailsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDeviceDto dto,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new CreateDeviceCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] UpdateDeviceDto dto,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new UpdateDeviceCommand(dto), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Deletea(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new DeleteDeviceCommand(id), cancellationToken);
        return Ok();
    }
}
