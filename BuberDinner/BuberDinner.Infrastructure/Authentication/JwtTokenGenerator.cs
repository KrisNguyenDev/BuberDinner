using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BuberDinner.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    public string GenerateToken(Guid userId, string firstName, string lastName)
    {
        // Tạo một mảng byte để lưu trữ khóa
        byte[] keyBytes = new byte[32]; // 256 bits / 8 bits/byte = 32 bytes

        // Sử dụng RNGCryptoServiceProvider để tạo ngẫu nhiên các byte cho khóa
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(keyBytes);
        }

        // Chuyển đổi mảng byte thành chuỗi hex để sử dụng làm khóa
        string key = BitConverter.ToString(keyBytes).Replace("-", "").ToLower();

        var siginingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var sercurityToken = new JwtSecurityToken(
            issuer: "BuberDinner",
            expires: _dateTimeProvider.UtcNow.AddMinutes(60),
            claims: claims,
            signingCredentials: siginingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(sercurityToken);
    }
}

