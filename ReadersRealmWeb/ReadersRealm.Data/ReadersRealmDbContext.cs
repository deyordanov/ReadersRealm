namespace ReadersRealm.Data;

using Common.Extensions.ModelBuilderExtensions;
using Microsoft.EntityFrameworkCore;
using Models;

public class ReadersRealmDbContext : DbContext
{
    public ReadersRealmDbContext(DbContextOptions<ReadersRealmDbContext> options)
        : base(options) { }

    public required DbSet<Category> Categories { get; set; }

    public required DbSet<Author> Authors { get; set; }

    public required DbSet<Book> Books { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
    }
}