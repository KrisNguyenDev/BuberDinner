using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        ErrorOr<AuthenticationResult> authResult = _authenticationService.Register(
            request.FirstName, 
            request.LastName, 
            request.Email, 
            request.PassWord);

        return authResult.Match(
            authResult => Ok(new AuthenticationResponse(
                authResult.user.Id,
                authResult.user.FirstName,
                authResult.user.LastName,
                authResult.user.Email,
                authResult.Token)
            ),
            firstError => Problem(statusCode: StatusCodes.Status409Conflict, title: firstError.de)
        );
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.login(request.Email, request.PassWord);

        var response = new AuthenticationResponse(
            authResult.user.Id, 
            authResult.user.FirstName, 
            authResult.user.LastName, 
            authResult.user.Email, 
            authResult.Token);

        return Ok(response);
    }
}