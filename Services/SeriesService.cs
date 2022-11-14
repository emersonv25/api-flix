using Api.MyFlix.Data;
using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Api.MyFlix.Services
{
    public class SeriesService : ISeriesService
    {
        private readonly AppDbContext _context;

        public SeriesService(AppDbContext context)
        {
            _context = context;

        }
        public async Task<ActionResult<IEnumerable<ReturnSeries>>> GetSerie()
        {
            var series = await _context.Serie.Include(m => m.Categories).ToListAsync();

            var returnSeries = new List<ReturnSeries>();

            if(series is not null)
            {
                foreach(var Serie in series)
                {
                    returnSeries.Add(new ReturnSeries(Serie));
                }
            }

            return returnSeries;
        }

        public async Task<ActionResult<ReturnSerie>> GetSerieById(int id)
        {
            var serie = await _context.Serie
                .Include(m => m.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes).FirstOrDefaultAsync(m => m.SerieId == id);

            if(serie is not null)
            {
               return new ReturnSerie(serie);
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult<ReturnSerie>> GetSerieByKey(string key)
        {
            var serie = await _context.Serie
                .Include(m => m.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes).FirstOrDefaultAsync(m => m.SerieKey == key);

            if (serie is not null)
            {
                return new ReturnSerie(serie);
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult<IEnumerable<ReturnSeries>>> SearchSerie(string name)
        {
            name = name.Trim();
            var series = await _context.Serie
                .Where(m => m.Title.Contains(name) || m.Description.Contains(name) || m.Categories.Select(c => c.Name).Contains(name))
                .Include(m => m.Categories).ToListAsync();

            var returnSeries = new List<ReturnSeries>();

            if (series is not null)
            {
                foreach (var serie in series)
                {
                    returnSeries.Add(new ReturnSeries(serie));
                }
            }
            return returnSeries;
        }
        public async Task<ActionResult> PostSerie(ParamSerie Serie)
        {
            var categories = _context.Category.Where(i => Serie.Categories.Contains(i.Name.ToLower())).ToList();

            var newCategories = Serie.Categories.Where(c => !categories.Select(x => x.Name).Contains(c)).ToList();

            categories = categories.Concat(newCategories.Select(c => new Category(c))).ToList();

            var newSerie = new Serie
            (
                Serie.Title,
                Serie.Description,
                Serie.PosterImg,
                Serie.ReleasedDate,
                Serie.Seasons,
                categories
            );
            if (SerieExistsByKey(newSerie.SerieKey))
            {
                return new BadRequestObjectResult($"{newSerie.SerieKey} já existe");
            }

            _context.Serie.Add(newSerie);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Cadastrado com Sucesso");
        }

        public async Task<ActionResult> PutSerie(int id, Serie serie)
        {
            if (id != serie.SerieId)
            {
                return new BadRequestObjectResult("O id é diferente do conteudo a ser editado");
            }

            _context.Entry(serie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SerieExists(id))
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

        public async Task<ActionResult> DeleteSerie(int id)
        {
            var serie = await _context.Serie.FindAsync(id);
            if (serie == null)
            {
                return new NotFoundObjectResult("Não encontrado");
            }

            _context.Serie.Remove(serie);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Deletado com sucesso");
        }

        private bool SerieExists(int id)
        {
            return _context.Serie.Any(e => e.SerieId == id);
        }
        private bool SerieExistsByKey(string key)
        {
            return _context.Serie.Any(e => e.SerieKey == key);
        }
    }
}
