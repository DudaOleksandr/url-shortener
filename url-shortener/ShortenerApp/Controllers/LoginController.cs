using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using url_shortener.ShortenerApp.Commons;
using url_shortener.ShortenerApp.Models.Dto;
using url_shortener.ShortenerApp.Models.Entities;

namespace url_shortener.ShortenerApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        
        if (user is null) return Unauthorized("Invalid username");
        
        var result = await _signInManager
            .PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
        
        if (!result.Succeeded) return Unauthorized("Invalid password");

        // Authentication successful, generate and return the authentication token
        var token = await GenerateToken(user);

        return Ok(new { Token = token });
    }

    /// <summary>
    /// Generate and return the authentication token (using JWT)
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task<string> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        
        claims.Add(new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? UserRoles.UserRole));

        //TODO get key from appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a secret that needs to be at least 16 characters long"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your-issuer",
            audience: "your-audience",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}