using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BarBeer.Models;

#nullable disable

namespace BarBeer.Context
{
    public partial class BarBeerContext : DbContext
    {
        public BarBeerContext()
        {
        }

        public BarBeerContext(DbContextOptions<BarBeerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bar> Bars { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<PersonalBestBar> PersonalBestBars { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=BarBeer;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Bar>(entity =>
            {
                entity.Property(e => e.BarImage)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BarLocation)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BarName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Comment1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Comment");

                entity.HasOne(d => d.Bar)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.BarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__BarId__52593CB8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__UserId__534D60F1");
            });

            modelBuilder.Entity<PersonalBestBar>(entity =>
            {
                entity.HasOne(d => d.Bar)
                    .WithMany(p => p.PersonalBestBars)
                    .HasForeignKey(d => d.BarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PersonalB__BarId__4E88ABD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PersonalBestBars)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PersonalB__UserI__4F7CD00D");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserLogin, "UQ__Users__7F8E8D5E05F21F02")
                    .IsUnique();

                entity.Property(e => e.UserLogin)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
