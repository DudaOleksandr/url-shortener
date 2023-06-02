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
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var user = new User
        {
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded) return BadRequest(result.Errors);
        
        if (model.IsAdmin)
        {
            await _userManager.AddToRoleAsync(user, UserRoles.AdminRole);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, UserRoles.UserRole);
        }

        return Ok();
    }
}