using Api.MyFlix.Data;
using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Api.MyFlix.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly AppDbContext _context;

        public MoviesService(AppDbContext context)
        {
            _context = context;

        }
        public async Task<ActionResult<IEnumerable<ReturnMovies>>> GetMovie()
        {
            var movies = await _context.Movie.Include(m => m.Categories).ToListAsync();

            var returnMovies = new List<ReturnMovies>();

            if(movies is not null)
            {
                foreach(var movie in movies)
                {
                    returnMovies.Add(new ReturnMovies(movie));
                }
            }
            return returnMovies;
        }

        public async Task<ActionResult<ReturnMovie>> GetMovieById(int id)
        {
            var movie = await _context.Movie
                .Include(m => m.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes).FirstOrDefaultAsync(m => m.MovieId == id);

            if(movie is not null)
            {
               return new ReturnMovie(movie);
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult<ReturnMovie>> GetMovieByKey(string key)
        {
            var movie = await _context.Movie
                .Include(m => m.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes).FirstOrDefaultAsync(m => m.MovieKey == key);

            if (movie is not null)
            {
                return new ReturnMovie(movie);
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult<IEnumerable<ReturnMovies>>> SearchMovie(string name)
        {
            name = name.Trim();
            var movies = await _context.Movie
                .Where(m => m.Title.Contains(name) || m.Description.Contains(name) || m.Categories.Select(c => c.Name).Contains(name))
                .Include(m => m.Categories).ToListAsync();

            var returnMovies = new List<ReturnMovies>();

            if (movies is not null)
            {
                foreach (var movie in movies)
                {
                    returnMovies.Add(new ReturnMovies(movie));
                }
            }
            return returnMovies;
        }
        public async Task<ActionResult> PostMovie(ParamMovie movie)
        {
            var categories = _context.Category.Where(i => movie.Categories.Contains(i.Name.ToLower())).ToList();

            var newCategories = movie.Categories.Where(c => !categories.Select(x => x.Name).Contains(c)).ToList();

            categories = categories.Concat(newCategories.Select(c => new Category(c))).ToList();

            var newMovie = new Movie
            (
                movie.Title,
                movie.Description,
                movie.PosterImg,
                movie.ReleasedDate,
                movie.Seasons,
                categories
            );
            if (MovieExistsByKey(newMovie.MovieKey))
            {
                return new BadRequestObjectResult($"{newMovie.MovieKey} já existe");
            }

            _context.Movie.Add(newMovie);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Cadastrado com Sucesso");
        }

        public async Task<ActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return new BadRequestObjectResult("O id é diferente do conteudo a ser editado");
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new OkObjectResult("Editado com sucesso");
        }

        public async Task<ActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return new NotFoundObjectResult("Não encontrado");
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Deletado com sucesso");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.MovieId == id);
        }
        private bool MovieExistsByKey(string key)
        {
            return _context.Movie.Any(e => e.MovieKey == key);
        }
    }
}
