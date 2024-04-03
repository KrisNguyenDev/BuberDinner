using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Index()
    {
        Exception? exption = HttpContext.Features.Get<ExceptionHandlerFeature>()?.Error;
        //return Problem(title: exption?.Message, statusCode: 400);
        return Problem();
    }
}
