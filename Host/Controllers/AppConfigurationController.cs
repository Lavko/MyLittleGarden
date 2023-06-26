using Application.Commands.AppConfiguration;
using Application.Queries.AppConfiguration;
using Domain.DTOs.AppConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class AppConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppConfigurationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppConfigurationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentAppConfigurationQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        [FromBody] AppConfigurationDto dto,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new UpdateAppConfigurationCommand(dto), cancellationToken);
        return Ok();
    }
}
