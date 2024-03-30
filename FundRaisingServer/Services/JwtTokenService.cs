using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FundRaisingServer.Configurations;
using FundRaisingServer.Models.DTOs.UserAuth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FundRaisingServer.Services;

public class JwtTokenService(IOptionsMonitor<JwtConfig> optionsMonitor)
{
    private readonly JwtConfig _jwtConfig = optionsMonitor.CurrentValue!;


    public string GenerateJwtToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        
        // getting the key
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        
        // specifying all the necessary information here
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }
}