using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using url_shortener.ShortenerApp.Models.Entities;

namespace url_shortener.ShortenerApp.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Url> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Create roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = Commons.UserRoles.AdminRole,
                NormalizedName = Commons.UserRoles.AdminRole.ToUpper() },
            new IdentityRole { Id = "2", Name = Commons.UserRoles.UserRole,
                NormalizedName = Commons.UserRoles.UserRole.ToUpper() }
        );
    }
}