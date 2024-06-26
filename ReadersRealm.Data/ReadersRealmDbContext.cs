﻿namespace ReadersRealm.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Extensions;
using Microsoft.AspNetCore.Identity;

public class ReadersRealmDbContext(DbContextOptions<ReadersRealmDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public required DbSet<Category> Categories { get; set; }

    public required DbSet<Author> Authors { get; set; }

    public required DbSet<Book> Books { get; set; }

    public required DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public required DbSet<Company> Companies { get; set; }

    public required DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public required DbSet<OrderHeader> OrderHeaders { get; set; }

    public required DbSet<OrderDetails> OrdersDetails { get; set; }

    public required DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}