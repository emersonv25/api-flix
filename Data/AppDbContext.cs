using Microsoft.EntityFrameworkCore;
using Api.MyFlix.Services;
using Api.MyFlix.Models;

namespace Api.MyFlix.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Episode> Espisode { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // USUARIO
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.Username).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique(true);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(true);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Enabled).HasDefaultValueSql("1").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Admin).HasDefaultValueSql("0").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).IsRequired();
            modelBuilder.Entity<User>()
                .HasData(
                    new User { UserId = 1, Username = "admin", FullName = "Administrador", Password = Utils.sha256_hash("admin"), Enabled = true, Admin = true, Email = "admin@admin.com" }
                );

            // Movie
            modelBuilder.Entity<Movie>().HasKey(m => m.MovieId);
            modelBuilder.Entity<Movie>().Property(m => m.Title).IsRequired();
            modelBuilder.Entity<Movie>().HasIndex(m => m.Title).IsUnique(true);
            modelBuilder.Entity<Movie>().HasIndex(m => m.MovieKey).IsUnique(true);
            modelBuilder.Entity<Movie>().Property(m => m.MovieKey).IsRequired();
            modelBuilder.Entity<Movie>().Property(m => m.ReleasedDate).HasMaxLength(4);

            // Season
            modelBuilder.Entity<Season>().HasKey(s => s.SeasonId);
            modelBuilder.Entity<Season>().Property(s => s.SeasonNum).IsRequired();
            modelBuilder.Entity<Season>().HasIndex(s => s.SeasonKey).IsUnique(true);
            modelBuilder.Entity<Season>().Property(s => s.SeasonKey).IsRequired();
            modelBuilder.Entity<Season>().HasOne(s => s.Movie).WithMany(m => m.Seasons).HasForeignKey(s => s.MovieId);
            modelBuilder.Entity<Season>().Property(s => s.MovieId).IsRequired();

            // Episodes
            modelBuilder.Entity<Episode>().HasKey(e => e.EpisodeId);
            modelBuilder.Entity<Episode>().Property(e => e.EpisodeNum).IsRequired(true);
            modelBuilder.Entity<Episode>().Property(e => e.Title).IsRequired(true);
            modelBuilder.Entity<Episode>().HasIndex(s => s.EpisodeKey).IsUnique(true);
            modelBuilder.Entity<Episode>().Property(s => s.EpisodeKey).IsRequired();
            modelBuilder.Entity<Episode>().Property(e => e.EpisodeUrl).IsRequired(true);


        }
    }
}