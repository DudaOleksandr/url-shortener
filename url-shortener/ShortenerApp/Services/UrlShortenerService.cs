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
    
    /// <inheritdoc />
    public string GetShortUrlToken(int id)
    {
        //Possible additional logic, hashing in separate method 
        return CreateHash(id);
    }
    
    /// <summary>
    /// Creates hash using id
    /// </summary>
    /// <param name="input">id</param>
    /// <returns>Hashed id</returns>
    private string CreateHash(int input)
    {
        return _hashIds.Encode(input); 
    }
}