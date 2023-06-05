namespace url_shortener.ShortenerApp.Models.Dto;

public class RegisterDto
{
    public string Username { get; set; }
    
    public string Password { get; set; }

    public bool IsAdmin { get; set; } = false;
}