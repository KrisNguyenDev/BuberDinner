using ErrorOr;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult login(string email, string password);
    ErrorOr<AuthenticationResult>  Register(string firstName, string lastName, string email, string password);
}

