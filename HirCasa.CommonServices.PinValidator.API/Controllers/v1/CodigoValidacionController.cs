using HirCasa.CommonServices.PinValidator.Business.UseCases.CodigoValidacion.Commands;
using HirCasa.CommonServices.PinValidator.Business.UseCases.CodigoValidacion.Queries;
using HirCasa.CommonServices.PinValidator.Business.UseCases.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HirCasa.CommonServices.PinValidator.API.Controllers.v1;

[ApiVersion("1.0")]
public class CodigoValidacionController : BaseController
{
    private readonly IMediator _mediator;

    public CodigoValidacionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("", Name = "CreateCodigoValidacion")]
    [ProducesResponseType(typeof(CodigoValidacionVm), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCodigoValidacion([FromBody] CreateCodigoValidacionCommand createCodigoValidacionCommand)
    {
        var result = await _mediator.Send(createCodigoValidacionCommand);

        return Ok(result);
    }

    [HttpPut("", Name = "ValidateCodigo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ValidateCodigo([FromBody] ValidateCodigoQuery validateCodigoQuery)
    {
        await _mediator.Send(validateCodigoQuery);

        return NoContent();
    }
}
