using System.ComponentModel.DataAnnotations;

namespace url_shortener.ShortenerApp.Models.Entities;

public class Url
{
    [Key]
    public int Id { get; set; }
    
    public string LongUrl { get; set; }
    
    public string ShortUrl { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public string CreatedBy { get; set; }
}