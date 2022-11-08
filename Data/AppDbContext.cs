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
            modelBuilder.Entity<Season>().Property(s => s.Number).IsRequired();
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

        private Movie getDefaultMovie()
        {

            return new Movie
            {
                MovieId = 5,
                MovieKey = "one-piece",
                Title = "One Piece",
                Description = "Houve um homem que conquistou tudo aquilo que o mundo tinha a oferecer, o lendário Rei dos Piratas, Gold Roger. Capturado e condenado à execução pelo Governo Mundial, suas últimas palavras lançaram legiões aos mares. “Meu tesouro? Se quiserem, podem pegá-lo. Procurem-no! Ele contém tudo que este mundo pode oferecer!”. Foi a revelação do maior tesouro, o One Piece, cobiçado por homens de todo o mundo, sonhando com fama e riqueza imensuráveis… Assim começou a Grande Era dos Piratas!",
                PosterImg = "https://animesbr.biz/wp-content/uploads/2019/06/oUPzuNw6e0NBbb121odI4ncCvRv-185x278.jpg",
                ReleasedDate = "1999",
                CreatedDate = DateTime.Now,
                Categories = new List<Category>
                    {
                        new Category { CategoryId = 1, Name = "Ação" },
                        new Category { CategoryId = 2, Name = "Aventura" },
                        new Category { CategoryId = 3, Name = "Drama" },
                    },
                Seasons = new List<Season>
                    {
                        new Season
                        {
                            SeasonId = 5,
                            SeasonKey = "one-piece-1-temporada",
                            Number = 1,
                            CreatedDate = DateTime.Now,
                            Episodes = new List<Episode>
                            {
                                new Episode
                                {
                                    EpisodeId = 1,
                                    EpisodeKey = "one-piece-episode-1",
                                    EpisodeNum = 1,
                                    Title = "Episódio 1 - Eu Sou Luffy! Aquele Que Será o Rei dos Piratas!",
                                    Description = "One Piece Episodio 1",
                                    EpisodeUrl = "https://www.blogger.com/video.g?token=AD6v5dwXwJxeialBSirm6ZXC1R1uhvC3XloL3ex93H7Ft1KGHVLcHHvCnaD4hI7BTFFawvaf6-31cVnI5bpWTNFq4gb8broG2lwhfJkYi2AGIvTDVfBtDv0w4bfSWU_uI7KmWGpQMz3A",
                                    CreatedDate = DateTime.Now,
                                }
                            }
                        }
                    }
            };
        }
    }
}