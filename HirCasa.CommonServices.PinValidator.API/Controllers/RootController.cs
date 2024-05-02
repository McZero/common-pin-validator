using Microsoft.AspNetCore.Mvc;

namespace HirCasa.CommonServices.PinValidator.API.Controllers;

[ApiController]
[Route("api/[controller]/")]
[Produces("application/json")]
public class BaseController : ControllerBase { }

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("/")]
public class RootController : BaseController
{
    private static readonly string[] values = new[]
    {
        ""
    };

    [HttpGet()]
    public ActionResult Get()
    {
        return Ok(values);
    }
}
