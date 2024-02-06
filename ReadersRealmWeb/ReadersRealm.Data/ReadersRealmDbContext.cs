namespace ReadersRealm.Data;

using Common;
using Common.Constants;
using Common.Extensions.ModelBuilderExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class ReadersRealmDbContext : IdentityDbContext<IdentityUser>
{
    public ReadersRealmDbContext(DbContextOptions<ReadersRealmDbContext> options)
        : base(options) { }

    public required DbSet<Category> Categories { get; set; }

    public required DbSet<Author> Authors { get; set; }

    public required DbSet<Book> Books { get; set; }

    public required DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public required DbSet<Company> Companies { get; set; }

    public required DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}