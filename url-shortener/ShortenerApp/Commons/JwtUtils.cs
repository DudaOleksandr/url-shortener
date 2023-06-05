using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using url_shortener.ShortenerApp.Models.Entities;

namespace url_shortener.ShortenerApp.Commons;

/// <summary>
/// Helper class to operate jwt tokens
/// </summary>
public class JwtUtils
{
    private readonly IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Generate and return the authentication token (using JWT)
    /// </summary>
    /// <param name="user">User entity, containing info about the user that is about to login</param>
    /// <param name="userRoles">Roles that belong to user</param>
    /// <returns>Jwt token</returns>
    public string GenerateToken(User user, IEnumerable<string> userRoles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            // Simplified logic, getting only first role, assuming that user will have only one
            new(ClaimTypes.Role, userRoles.FirstOrDefault() ?? UserRoles.UserRole)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public (string? userId, string? userName) GetTokenInfo(string modelJwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(modelJwtToken);
        var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        
        return (userId, userName);
    }
}