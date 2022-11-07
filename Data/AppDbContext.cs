using Microsoft.EntityFrameworkCore;
using MyFlix.Services;
using MyFlix.Models;

namespace MyFlix.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
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
            modelBuilder.Entity<Movie>().Property(u => u.LaunchDate).HasMaxLength(4);
            modelBuilder.Entity<Movie>().Property(u => u.Rating).HasMaxLength(4);


           // modelBuilder.Entity<Movie>().HasData(getDefaultMovie());

            // Season
            modelBuilder.Entity<Season>().HasKey(s => s.SeasonId);
            modelBuilder.Entity<Season>().Property(s => s.Number).IsRequired();
            modelBuilder.Entity<Season>().HasOne(s => s.Movie).WithMany(m => m.Seasons).HasForeignKey(s => s.MovieId);

            // Episodes
            modelBuilder.Entity<Episode>().HasKey(e => e.EpisodeId);
            modelBuilder.Entity<Episode>().Property(e => e.Number).IsRequired(true);
            modelBuilder.Entity<Episode>().Property(e => e.Title).IsRequired(true);
            modelBuilder.Entity<Episode>().Property(e => e.VideoUrl).IsRequired(true);


        }

        private Movie getDefaultMovie()
        {
            return new Movie
            {
                MovieId = 1,
                Title = "One Piece",
                Description = "Houve um homem que conquistou tudo aquilo que o mundo tinha a oferecer, o lendário Rei dos Piratas, Gold Roger. Capturado e condenado à execução pelo Governo Mundial, suas últimas palavras lançaram legiões aos mares. “Meu tesouro? Se quiserem, podem pegá-lo. Procurem-no! Ele contém tudo que este mundo pode oferecer!”. Foi a revelação do maior tesouro, o One Piece, cobiçado por homens de todo o mundo, sonhando com fama e riqueza imensuráveis… Assim começou a Grande Era dos Piratas!",
                Rating = "10.0",
                PosterUrl = "https://animesbr.biz/wp-content/uploads/2019/06/oUPzuNw6e0NBbb121odI4ncCvRv-185x278.jpg",
                LaunchDate = "1999",
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
                            SeasonId = 1,
                            Number = 1,
                            MovieId = 1,
                            Episodes = new List<Episode>
                            {
                                new Episode
                                {
                                    EpisodeId = 1,
                                    Number = 1,
                                    Title = "Episódio 1 - Eu Sou Luffy! Aquele Que Será o Rei dos Piratas!",
                                    Description = "One Piece Episodio 1",
                                    VideoUrl = "https://www.blogger.com/video.g?token=AD6v5dwXwJxeialBSirm6ZXC1R1uhvC3XloL3ex93H7Ft1KGHVLcHHvCnaD4hI7BTFFawvaf6-31cVnI5bpWTNFq4gb8broG2lwhfJkYi2AGIvTDVfBtDv0w4bfSWU_uI7KmWGpQMz3A"
                                }
                            }
                        }
                    }
            };
        }

        public DbSet<MyFlix.Models.Movie> Movie { get; set; }

    }
}