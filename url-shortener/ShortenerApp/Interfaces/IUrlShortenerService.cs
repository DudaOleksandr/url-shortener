namespace url_shortener.ShortenerApp.Interfaces;

public interface IUrlShortenerService
{
    public string GetShortUrlToken(int id);
}