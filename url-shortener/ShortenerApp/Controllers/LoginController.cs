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
    private readonly IConfiguration _configuration;

    public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }
    
    /// <summary>
    /// Authenticates a user with the provided credentials and generates an authentication token.
    /// </summary>
    /// <param name="model">The login model containing the username and password.</param>
    /// <returns>An authentication token if the login is successful.</returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        // Invalid input, return bad request with validation errors
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(model.Username);

        if (user is null) return Unauthorized("Invalid username or password");

        var result = await _signInManager
            .PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded) return Unauthorized("Invalid username or password");
        
        // Authentication successful, generate and return the authentication token
        var token = await GenerateToken(user);

        return Ok(new { Token = token });
    }


    /// <summary>
    /// Generate and return the authentication token (using JWT)
    /// </summary>
    /// <param name="user">User entity, containing info about the user that is about to login</param>
    /// <returns>Jwt token</returns>
    private async Task<string> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        
        // Simplified logic, getting only first role, assuming that user will have only one
        claims.Add(new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? UserRoles.UserRole));
        
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
}