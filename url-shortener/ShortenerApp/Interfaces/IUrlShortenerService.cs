namespace url_shortener.ShortenerApp.Interfaces;

public interface IUrlShortenerService
{
    /// <summary>
    /// Creates short url version based on long version
    /// </summary>
    /// <returns>Short url version</returns>
    public string GetShortUrlToken(int id);
}