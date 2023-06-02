using Microsoft.EntityFrameworkCore;
using url_shortener.ShortenerApp.Models.Entities;

namespace url_shortener.ShortenerApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Url> Urls { get; set; }
}