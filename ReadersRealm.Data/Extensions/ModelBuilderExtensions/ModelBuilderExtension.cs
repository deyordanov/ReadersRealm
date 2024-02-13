namespace ReadersRealm.Common.Extensions.ModelBuilderExtensions;

using Data.Models;
using Microsoft.EntityFrameworkCore;
using ReadersRealm.Data.Models.Enums;
using System;
using Data.Configurations;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<Category>(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration<Author>(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration<Book>(new BookConfiguration());
    }
}