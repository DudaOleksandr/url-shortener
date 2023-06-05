using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    private readonly JwtUtils _jwtUtils;

    public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, JwtUtils jwtUtils)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtUtils = jwtUtils;
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
        
        var roles = await _userManager.GetRolesAsync(user);

        // Authentication successful, generate and return the authentication token
        var token =  _jwtUtils.GenerateToken(user, roles);

        return Ok(new { Token = token });
    }
}