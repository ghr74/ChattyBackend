using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChattyBackend.Models;
using ChattyBackend.Models.Enum;
using Microsoft.IdentityModel.Tokens;

namespace ChattyBackend.Helpers;

public sealed class TokenService(ILogger<TokenService> logger)
{
    private readonly string _secret = File.ReadAllText("toucan.txt");

    public string CreateToken(AuthUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = new JwtSecurityToken(
            claims: [new Claim("role", Role.User.ToString()), new Claim("id", user.Id.ToString())],
            expires: DateTime.UtcNow.AddDays(3),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                SecurityAlgorithms.HmacSha256Signature
            )
        );
        return tokenHandler.WriteToken(token);
    }

    public string CreateToken2(AuthUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [new Claim("role", Role.User.ToString()), new Claim("id", user.Id.ToString())]
            ),
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    //
    // private string GenerateEntropyToken()
    // {
    //     var rng = RandomNumberGenerator.GetBytes(32);
    //
    //     return Convert.ToBase64String(rng);
    // }
}
