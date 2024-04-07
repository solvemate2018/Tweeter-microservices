using GatewayService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GatewayService.Services
{
    public class JwtUtils
    {
        public static string GenerateJwtToken(User user, IConfiguration configuration)
        {
            var securityKey = GetSigningKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Name, user.LastName),
        };

            var token = new JwtSecurityToken(
                issuer: "auth-service",
                audience: "tweeter-services",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4), // Adjust expiration time as needed
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static SymmetricSecurityKey GetSigningKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My_Not_That_Little_Secret_That_Should_Be_Large_Enough_Now"));
        }

    }
}
