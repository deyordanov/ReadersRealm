﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReadersRealm.Data;

#nullable disable

namespace ReadersRealm.Migrations
{
    [DbContext(typeof(ReadersRealmDbContext))]
    [Migration("20240201185927_AddedAuthorAndBookTablesToDb")]
    partial class AddedAuthorAndBookTablesToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ReadersRealm.Data.Models.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Author Identifier");

                    b.Property<int?>("Age")
                        .HasColumnType("int")
                        .HasComment("Author Age");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)")
                        .HasComment("Author Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasComment("Author First Name");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasComment("Author Gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasComment("Author Last Name");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasComment("Author Middle Name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasComment("Author Phone Number");

                    b.HasKey("Id");

                    b.ToTable("Authors", t =>
                        {
                            t.HasComment("Readers Realm Author");
                        });
                });

            modelBuilder.Entity("ReadersRealm.Data.Models.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Book Identifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Author Identifier");

                    b.Property<int?>("BookCover")
                        .HasColumnType("int")
                        .HasComment("Book Cover Type");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("Book Description");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)")
                        .HasComment("Book ISBN");

                    b.Property<int?>("Pages")
                        .HasColumnType("int")
                        .HasComment("Book Page Count");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasComment("Book Price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Book Title");

                    b.Property<bool?>("Used")
                        .HasColumnType("bit")
                        .HasComment("Book Condition");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books", t =>
                        {
                            t.HasComment("Readers Realm Book");
                        });
                });

            modelBuilder.Entity("ReadersRealm.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Category Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid?>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int")
                        .HasComment("Category Display Order");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Category Name");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Categories", t =>
                        {
                            t.HasComment("Readers Realm Category");
                        });

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Action"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "SciFi"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "History"
                        });
                });

            modelBuilder.Entity("ReadersRealm.Data.Models.Book", b =>
                {
                    b.HasOne("ReadersRealm.Data.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ReadersRealm.Data.Models.Category", b =>
                {
                    b.HasOne("ReadersRealm.Data.Models.Book", null)
                        .WithMany("Categories")
                        .HasForeignKey("BookId");
                });

            modelBuilder.Entity("ReadersRealm.Data.Models.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("ReadersRealm.Data.Models.Book", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
