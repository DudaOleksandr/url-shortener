using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    private readonly AppDbContext _context;

    public ShortUrlController(IUrlShortenerService urlShortenerService, AppDbContext context)
    {
        _urlShortenerService = urlShortenerService;
        _context = context;
    }

    /// <summary>
    /// Creates a short URL from the provided original URL.
    /// </summary>
    /// <param name="model">The model containing the original URL and creator name.</param>
    /// <returns>Result of creating short URL.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] ShortUrlDto model)
    {
        // Validate the model
        if (!ModelState.IsValid) return BadRequest(ModelState);

        //Validate Url
        if (!Uri.TryCreate(model.OriginalUrl, UriKind.Absolute, out _)) return BadRequest("Invalid Url");

        // Save the short URL to the database
        var shortUrl = new Url
        {
            LongUrl = model.OriginalUrl,
            CreatedDate = DateTime.Now,
            CreatedBy = model.CreatorName
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
    /// Retrieves the details of a specific short URL by its ID.
    /// </summary>
    /// <param name="id">The ID of the short URL.</param>
    /// <returns>The short URL details.</returns>
    [HttpGet("{id}")]
    public IActionResult GetShortUrl(int id)
    {
        // Retrieve the short URL from the database by ID
        var shortUrl = _context.Urls.FirstOrDefault(u => u.Id == id);

        if (shortUrl is null) return NotFound($"No Url was found with Id: {id}");
        
        // Return the short URL details
        return Ok(shortUrl);
    }

    /// <summary>
    /// Deletes a short URL by its ID.
    /// </summary>
    /// <param name="id">The ID of the short URL.</param>
    /// <returns>A success message indicating the short URL was deleted.</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteShortUrl(int id)
    {
        // Find the short URL in the database by ID
        var shortUrl = _context.Urls.FirstOrDefault(u => u.Id == id);

        if (shortUrl is null) return NotFound($"No Url was found with Id: {id}");
        

        // Delete the short URL
        _context.Urls.Remove(shortUrl);
        _context.SaveChanges();

        // Return a success message
        return Ok("Short URL deleted successfully.");
    }
}
