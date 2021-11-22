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
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BarLocation)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BarName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Comment");

                entity.HasOne(d => d.Bar)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.BarId)
                    .HasConstraintName("FK__Comments__BarId__656C112C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Comments__UserId__66603565");
            });

            modelBuilder.Entity<PersonalBestBar>(entity =>
            {
                entity.HasOne(d => d.Bar)
                    .WithMany(p => p.PersonalBestBars)
                    .HasForeignKey(d => d.BarId)
                    .HasConstraintName("FK__PersonalB__BarId__619B8048");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PersonalBestBars)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PersonalB__UserI__628FA481");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserLogin, "UQ__Users__7F8E8D5E7183C904")
                    .IsUnique();

                entity.Property(e => e.UserLogin)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
