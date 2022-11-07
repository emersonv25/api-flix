using Api.MyFlix.Data;
using Api.MyFlix.Models;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.MyFlix.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly AppDbContext _context;

        public MoviesService(AppDbContext context)
        {
            _context = context;

        }
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            return await _context.Movie.Include(m => m.Categories).ToListAsync();
        }

        public async Task<ActionResult<Movie?>> GetMovie(int id)
        {
            var movie = await _context.Movie
                .Include(m => m.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes).FirstOrDefaultAsync(m => m.MovieId == id);
            return movie;
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
        public async Task<ActionResult> PostMovie(Movie movie)
        {
            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Cadastrado com Sucesso");
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

    }
}
