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
        public async Task<ActionResult<Result>> GetSerie(string search, int currentPage, int pageSize, string sortOrder)
        {
            #region pagination
            int count = await _context.Serie.CountAsync();
            int skip = (currentPage - 1) * pageSize;
            int take = pageSize;
            #endregion
            List<Serie> series;

            if (string.IsNullOrWhiteSpace(search))
            {
                series = await _context.Serie
                .Include(m => m.Categories)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            }
            else
            {
                series = await _context.Serie
                .Where(m => m.Title.Contains(search) || m.Description.Contains(search) || m.Categories.Select(c => c.Name).Contains(search))
                .Skip(skip)
                .Take(take)
                .Include(m => m.Categories).ToListAsync();
            }

            var returnSeries = new List<ReturnSeries>();

            if (series is not null)
            {
                foreach (var Serie in series)
                {
                    returnSeries.Add(new ReturnSeries(Serie));
                }
            }

            #region filters
            switch (sortOrder)
            {
                case "title_asc":
                    returnSeries = returnSeries.OrderBy(r => r.Title).ToList();
                    break;
                case "title_desc":
                    returnSeries = returnSeries.OrderByDescending(r => r.Title).ToList();
                    break;
                case "date_asc":
                    returnSeries = returnSeries.OrderBy(r => r.ReleasedDate).ToList();
                    break;
                case "date_desc":
                    returnSeries = returnSeries.OrderByDescending(r => r.ReleasedDate).ToList();
                    break;
            }
            #endregion

            Result result = new Result(returnSeries.ToList<dynamic>(), count, currentPage, pageSize);
            return result;
        }

        public async Task<ActionResult<ReturnSerie>> GetSerieById(int id)
        {
            var serie = await _context.Serie
                .Include(m => m.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes).FirstOrDefaultAsync(m => m.SerieId == id);

            if (serie is not null)
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
                serie.Views += 1;
                _context.SaveChanges();
                return new ReturnSerie(serie);
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult> PostSerie(ParamSerie Serie)
        {
            var categories = _context.Category.Where(i => Serie.Categories.Contains(i.Name.ToUpper())).ToList();

            var newCategories = Serie.Categories.Where(c => !categories.Select(x => x.Name.ToUpper()).Contains(c.ToUpper())).ToList();

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
