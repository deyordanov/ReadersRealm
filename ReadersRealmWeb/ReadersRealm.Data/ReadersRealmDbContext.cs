namespace ReadersRealm.Data;

using Common.Extensions.ModelBuilderExtensions;
using Microsoft.EntityFrameworkCore;
using Models;

public class ReadersRealmDbContext : DbContext
{
    public ReadersRealmDbContext(DbContextOptions<ReadersRealmDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
    }
}