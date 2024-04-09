using BuberDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Index()
    {
        Exception? exption = HttpContext.Features.Get<ExceptionHandlerFeature>()?.Error;

        var (statusCode, message) = exption switch
        {
            DuplicateEmailExeption => (StatusCodes.Status409Conflict, "Email already exists"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        return Problem(statusCode: statusCode, title: message);
    }
}
