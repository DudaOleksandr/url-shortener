using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace url_shortener.ShortenerApp.Models.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Password { get; set; }

    public bool IsAdmin { get; set; }
}