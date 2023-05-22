using Application.Commands.Outlets;
using Application.Queries.Outlets;
using Domain.Configurations;
using Domain.DTOs;
using Domain.Repositories.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Host.Controllers;

[ApiController]
[Route("[controller]")]
public class OutletsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OutletsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetAllOutletsQuery(), cancellationToken);

        return Ok(items);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update([FromBody]UpdateOutletDto updateOutletDto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateOutletCommand(updateOutletDto), cancellationToken);

        return Ok();
    }
}