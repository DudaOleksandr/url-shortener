using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using url_shortener.ShortenerApp.Commons;
using url_shortener.ShortenerApp.Data;
using url_shortener.ShortenerApp.Interfaces;
using url_shortener.ShortenerApp.Models.Dto;
using url_shortener.ShortenerApp.Models.Entities;

namespace url_shortener.ShortenerApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShortUrlController : ControllerBase
{
    private readonly IUrlShortenerService _urlShortenerService;
    private readonly UserManager<User> _userManager;
    private readonly JwtUtils _jwtUtils;
    private readonly AppDbContext _context;

    public ShortUrlController(IUrlShortenerService urlShortenerService, AppDbContext context,
        UserManager<User> userManager, JwtUtils jwtUtils)
    {
        _urlShortenerService = urlShortenerService;
        _context = context;
        _userManager = userManager;
        _jwtUtils = jwtUtils;
    }

    /// <summary>
    /// Creates a short URL from the provided original URL.
    /// </summary>
    /// <param name="model">The model containing the original URL and creator name.</param>
    /// <param name="authorization">The jwt token.</param>
    /// <returns>Result of creating short URL.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] ShortUrlDto model, [FromHeader] string authorization)
    {
        // Validate the model
        if (!ModelState.IsValid) return BadRequest(ModelState);

        //Validate Url
        if (!Uri.TryCreate(model.OriginalUrl, UriKind.Absolute, out _)) return BadRequest("Invalid Url");

        var (userId, userName) = _jwtUtils.GetTokenInfo(authorization);

        if (userId is null || userName is null) return Forbid();

        if (_context.Urls.FirstOrDefault(x => x.LongUrl == model.OriginalUrl) != null)
            return BadRequest("Url already exists");

        // Save the short URL to the database
        var shortUrl = new Url
        {
            LongUrl = model.OriginalUrl,
            CreatedDate = DateTime.Now,
            CreatedBy = userName,
            CreatorId = userId
        };
        
        //TODO Rewrite
        var url = _context.Urls.Add(shortUrl);
        await _context.SaveChangesAsync();
        // Generate a short URL code
        url.Entity.ShortUrl = _urlShortenerService.GetShortUrlToken(url.Entity.Id);
        await _context.SaveChangesAsync();
        
        // Return the created short URL
        return Ok(shortUrl);
    }

    /// <summary>
    /// Retrieves all short URLs.
    /// </summary>
    /// <returns>A list of short URLs.</returns>
    [HttpGet]
    public IActionResult GetAllShortUrls()
    {
        // Retrieve all short URLs from the database
        var shortUrls = _context.Urls.ToList();

        if (shortUrls.IsNullOrEmpty()) return NotFound("No urls were found");
        
        // Return the list of short URLs
        return Ok(shortUrls);
    }

    /// <summary>
    /// Retrieves the details of a specific short URL by its Token.
    /// </summary>
    /// <param name="token">The token of the short URL.</param>
    /// <returns>The short URL details.</returns>
    [HttpGet("{token}")]
    public IActionResult GetShortUrl(string token)
    {
        // Retrieve the short URL from the database by ID
        var shortUrl = _context.Urls.FirstOrDefault(u => u.ShortUrl == token);

        if (shortUrl is null) return NotFound($"No Url was found with Id: {token}");
        
        // Return the short URL details
        return Ok(shortUrl);
    }

    /// <summary>
    /// Deletes a short URL by its ID.
    /// </summary>
    /// <param name="id">The ID of the short URL.</param>
    /// <param name="authorization">The jwt token.</param>
    /// <returns>A success message indicating the short URL was deleted.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShortUrl(int id, [FromHeader] string authorization)
    {
        // Find the short URL in the database by ID
        var shortUrl = _context.Urls.FirstOrDefault(u => u.Id == id);

        if (shortUrl is null) return NotFound($"No Url was found with Id: {id}");
        
        var (userId, _) = _jwtUtils.GetTokenInfo(authorization);

        if (userId is null) return BadRequest("User is not authorized");
        
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return NotFound("No such user found");

        var isAdminUser = (await _userManager.GetRolesAsync(user)).Contains(UserRoles.AdminRole);

        if (!isAdminUser && shortUrl.CreatorId != user.Id) return Forbid();
        
        // Delete the short URL
        _context.Urls.Remove(shortUrl);
        await _context.SaveChangesAsync();

        // Return a success message
        return Ok("Short URL deleted successfully.");
    }
    
    /// <summary>
    /// Deletes all short URLs.
    /// </summary>
    /// <param name="authorization">The jwt token.</param>
    /// <returns>A success message indicating the short URL was deleted.</returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteAllShortUrl([FromHeader] string authorization)
    {
        // Find the short URL in the database by ID
        var shortUrl = _context.Urls;

        if (shortUrl.IsNullOrEmpty()) return NotFound($"No Urls were found");
        
        var (userId, _) = _jwtUtils.GetTokenInfo(authorization);

        if (userId is null) return BadRequest("User is not authorized");
        
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return NotFound("No such user found");

        var isAdminUser = (await _userManager.GetRolesAsync(user)).Contains(UserRoles.AdminRole);

        if (!isAdminUser) return Forbid();
        
        // Delete all short URLs
        await _context.Urls.ForEachAsync(x=> _context.Urls.Remove(x));
        await _context.SaveChangesAsync();

        // Return a success message
        return Ok("Short URLs deleted successfully.");
    }
}
