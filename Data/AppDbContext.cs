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
        public DbSet<Serie> Serie { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<EpisodeVideo> EpisodeVideos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // USUARIO
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.Username).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique(true);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(true);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Enabled).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Admin).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).IsRequired();
            modelBuilder.Entity<User>()
                .HasData(
                    new User { UserId = 1, Username = "admin", FullName = "Administrador", Password = Utils.sha256_hash("admin"), Enabled = true, Admin = true, Email = "admin@admin.com" }
                );

            // Serie
            modelBuilder.Entity<Serie>().HasKey(m => m.SerieId);
            modelBuilder.Entity<Serie>().Property(m => m.Title).IsRequired();
            modelBuilder.Entity<Serie>().HasIndex(m => m.Title).IsUnique(true);
            modelBuilder.Entity<Serie>().HasIndex(m => m.SerieKey).IsUnique(true);
            modelBuilder.Entity<Serie>().Property(m => m.SerieKey).IsRequired();
            modelBuilder.Entity<Serie>().Property(m => m.ReleasedDate).HasMaxLength(4);

            // Season
            modelBuilder.Entity<Season>().HasKey(s => s.SeasonId);
            modelBuilder.Entity<Season>().Property(s => s.SeasonNum).IsRequired();
            modelBuilder.Entity<Season>().HasIndex(s => s.SeasonKey).IsUnique(true);
            modelBuilder.Entity<Season>().Property(s => s.SeasonKey).IsRequired();
            modelBuilder.Entity<Season>().HasOne(s => s.Serie).WithMany(m => m.Seasons).HasForeignKey(s => s.SerieId);
            modelBuilder.Entity<Season>().Property(s => s.SerieId).IsRequired();

            // Episodes
            modelBuilder.Entity<Episode>().HasKey(e => e.EpisodeId);
            modelBuilder.Entity<Episode>().Property(e => e.EpisodeNum).IsRequired(true);
            modelBuilder.Entity<Episode>().Property(e => e.Title).IsRequired(true);
            modelBuilder.Entity<Episode>().HasIndex(e => e.EpisodeKey).IsUnique(true);
            modelBuilder.Entity<Episode>().Property(e => e.EpisodeKey).IsRequired();
            modelBuilder.Entity<Episode>().HasOne(e => e.Season).WithMany(s => s.Episodes).HasForeignKey(e => e.SeasonId);

            // EpisodeVideo
            modelBuilder.Entity<EpisodeVideo>().HasKey(v => v.VideoId);
            modelBuilder.Entity<EpisodeVideo>().Property(v => v.VideoUrl).IsRequired(true);
            modelBuilder.Entity<EpisodeVideo>().HasOne(v => v.Episode).WithMany(e => e.EpisodeVideos).HasForeignKey(v => v.EpisodeId);


        }
    }
}