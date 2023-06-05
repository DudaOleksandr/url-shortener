using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using url_shortener.ShortenerApp.Commons;
using url_shortener.ShortenerApp.Models.Dto;
using url_shortener.ShortenerApp.Models.Entities;

namespace url_shortener.ShortenerApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public RegisterController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    /// <summary>
    /// Registers a user with the provided credentials.
    /// </summary>
    /// <param name="model">The register model containing the username and password.</param>
    /// <returns>Result of registration.</returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var user = new User
        {
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded) return BadRequest(result.Errors);
        
        //Add roles
        if (model.IsAdmin)
        {
            await _userManager.AddToRoleAsync(user, UserRoles.AdminRole);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, UserRoles.UserRole);
        }

        // Registration successful
        return Ok(new { user.UserName, user.Id });
    }
}