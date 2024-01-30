namespace ReadersRealm.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class ReadersRealmDbContext : DbContext
{
    public ReadersRealmDbContext(DbContextOptions<ReadersRealmDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; } = null!;
}