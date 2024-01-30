namespace ReadersRealm.Data;

using Extensions.ModelBuilderExtensions;
using Microsoft.EntityFrameworkCore;
using ReadersRealm.DataModels;

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