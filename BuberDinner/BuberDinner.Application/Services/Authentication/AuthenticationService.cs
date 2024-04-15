using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using FluentResults;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }
    public AuthenticationResult login(string email, string password)
    {
        // 1. Kiểm tra email đúng ko
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("Tài khoản không tồn tại.");
        }
        // 2. Kiểm tra password ko
        if (user.Password != password)
        {
            throw new Exception("Mật khẩu sai.");
        }

        // 3. Tạo JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // 1. Kiểm tra tài khoản không tồn tại
        if (_userRepository.GetUserByEmail(email) is null)
        {
            return Errors.User.DuplicateEmail;
        }

        // 2. Tạo user
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
        _userRepository.Add(user);

        // 3. Tạo  JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}

