using url_shortener.ShortenerApp.Interfaces;
using HashidsNet;

namespace url_shortener.ShortenerApp.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly Hashids _hashIds;
    public UrlShortenerService(IConfiguration configuration)
    {
        _hashIds = new Hashids(configuration["HashId:Salt"]);
    }

    /// <summary>
    /// Creates short url version based on long version
    /// </summary>
    /// <returns>Short url version</returns>
    public string GetShortUrlToken(int id)
    {
        //Possible additional logic, hashing in separate method 
        return CreateHash(id);
    }
    
    private string CreateHash(int input)
    {
        return _hashIds.Encode(input); 
    }
}