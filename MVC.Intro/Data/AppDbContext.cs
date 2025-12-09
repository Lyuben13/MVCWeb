using Microsoft.EntityFrameworkCore;
using MVC.Intro.Models;

namespace MVC.Intro.Data
{
    /// <summary>
    /// Database context for the application
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Products table
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Configure entity relationships and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);
                entity.HasIndex(e => e.Email)
                    .IsUnique();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.PasswordHash)
                    .IsRequired();
            });
        }
    }
}