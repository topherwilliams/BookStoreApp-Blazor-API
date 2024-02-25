using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : DbContext
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__3213E83F464BCAF3");

            entity.ToTable("Author");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bio)
                .HasMaxLength(250)
                .HasColumnName("bio");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3213E83FC8AFC540");

            entity.ToTable("Book");

            entity.HasIndex(e => e.Isbn, "UQ__Book__447D36EA17B0A32A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .HasColumnName("image");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Summary)
                .HasMaxLength(250)
                .HasColumnName("summary");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Book_ToAuthor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
