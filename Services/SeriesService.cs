using Api.MyFlix.Data;
using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Drawing.Printing;

namespace Api.MyFlix.Services
{
    public class SeriesService : ISeriesService
    {
        private readonly AppDbContext _context;

        public SeriesService(AppDbContext context)
        {
            _context = context;

        }
        public async Task<ActionResult<Result>> GetSerie(int currentPage, int pageSize)
        {
            #region paginação
            int count = await _context.Serie.CountAsync();
            int skip = (currentPage - 1) * pageSize;
            int take = pageSize;
            #endregion

            var series = await _context.Serie
                .Include(m => m.Categories)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var returnSeries = new List<ReturnSeries>();

            if (series is not null)
            {
                foreach (var Serie in series)
                {
                    returnSeries.Add(new ReturnSeries(Serie));
                }
            }

            #region Retorno
            Result result = new Result();
            result.TotalResults = count;
            result.CurrentPage = currentPage;
            result.ItemsPerPage = pageSize;
            result.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            result.HasPreviousPage = currentPage > 1;
            result.HasNextPage = currentPage < result.TotalPages;
            result.Results = returnSeries.ToList<dynamic>();
            #endregion
            return result;
        }
        public async Task<ActionResult<Result>> SearchSerie(string search, int currentPage, int pageSize)
        {
            #region paginação
            int count = await _context.Serie
                .Where(m => m.Title.Contains(search) || m.Description.Contains(search) || m.Categories.Select(c => c.Name).Contains(search))
                .CountAsync();
            int skip = (currentPage - 1) * pageSize;
            int take = pageSize;
            #endregion

            search = search.Trim();
            var series = await _context.Serie
                .Where(m => m.Title.Contains(search) || m.Description.Contains(search) || m.Categories.Select(c => c.Name).Contains(search))
                .Skip(skip)
                .Take(take)
                .Include(m => m.Categories).ToListAsync();

            var returnSeries = new List<ReturnSeries>();

            if (series is not null)
            {
                foreach (var serie in series)
                {
                    returnSeries.Add(new ReturnSeries(serie));
                }
            }

            #region Retorno
            Result result = new Result();
            result.TotalResults = count;
            result.CurrentPage = currentPage;
            result.ItemsPerPage = pageSize;
            result.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            result.HasPreviousPage = currentPage > 1;
            result.HasNextPage = currentPage < result.TotalPages;
            result.Results = returnSeries.ToList<dynamic>();
            #endregion
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
