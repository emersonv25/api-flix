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
        private readonly IConfiguration _configuration;

        public SeriesService(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;

        }
        public async Task<ActionResult<Result>> GetSerie(string search, string keys,int currentPage, int pageSize, string sortOrder)
        {
            #region pagination
            int count = 0;
            int skip = (currentPage - 1) * pageSize;
            int take = pageSize;
            #endregion

            #region sort
            string columnOrder = "Title";
            bool isAsc = true;
            switch (sortOrder)
            {
                case "title":
                    columnOrder = "Title";
                    break;
                case "latest_release":
                    columnOrder = "LatestRelease";
                    isAsc = false;
                    break;
                case "created_date":
                    columnOrder = "CreatedDate";
                    isAsc = false;
                    break;
                case "released_date":
                    columnOrder = "ReleasedDate";
                    isAsc = false;
                    break;
                case "most_view":
                    columnOrder = "Views";
                    isAsc = false;
                    break;
            }
            #endregion

            List<Serie> series;
            if(!string.IsNullOrWhiteSpace(keys))
            {
                var listKeys = keys.Split(';').ToList();
                if (isAsc)
                {
                    series = await _context.Serie.Include(m => m.Categories).Where(s => listKeys.Contains(s.SerieKey)).OrderBy(p => EF.Property<object>(p, columnOrder)).Skip(skip).Take(take).ToListAsync();
                }
                else
                {
                    series = await _context.Serie.Include(m => m.Categories).Where(s => listKeys.Contains(s.SerieKey)).OrderByDescending(p => EF.Property<object>(p, columnOrder)).Skip(skip).Take(take).ToListAsync();
                }

                count = await _context.Serie.Include(m => m.Categories).Where(s => listKeys.Contains(s.SerieKey)).CountAsync();
            }
            else if (string.IsNullOrWhiteSpace(search))
            {
                if (isAsc)
                {
                    series = await _context.Serie.Include(m => m.Categories).OrderBy(p => EF.Property<object>(p, columnOrder)).Skip(skip).Take(take).ToListAsync();
                }
                else
                {
                    series = await _context.Serie.Include(m => m.Categories).OrderByDescending(p => EF.Property<object>(p, columnOrder)).Skip(skip).Take(take).ToListAsync();
                }

                count = await _context.Serie.CountAsync();
            }
            else
            {
                if (isAsc)
                {
                    series = await _context.Serie
                        .Include(m => m.Categories)
                        .Where(m => m.Title.Contains(search) || m.Description.Contains(search) || m.Categories.Select(c => c.Name).Contains(search))
                        .OrderBy(p => EF.Property<object>(p, columnOrder))
                        .Skip(skip).Take(take).ToListAsync();
                }
                else
                {
                    series = await _context.Serie
                        .Include(m => m.Categories)
                        .Where(m => m.Title.Contains(search) || m.Description.Contains(search) || m.Categories.Select(c => c.Name).Contains(search))
                        .OrderByDescending(p => EF.Property<object>(p, columnOrder))
                        .Skip(skip).Take(take).ToListAsync();
                }

                count = await _context.Serie.Where(m => m.Title.Contains(search) || m.Description.Contains(search) || m.Categories.Select(c => c.Name).Contains(search)).CountAsync();
            }

            var returnSeries = new List<ReturnSeries>();

            if (series is not null)
            {
                foreach (var serie in series)
                {
                    returnSeries.Add(new ReturnSeries(GetImageUrlSerie(serie)));
                }
            }

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
                return new ReturnSerie(GetImageUrlSerie(serie));
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult> PostSerie(ParamSerie paramSerie)
        {
            var categories = _context.Category.Where(i => paramSerie.Categories.Contains(i.Name.ToUpper())).ToList();

            var newCategories = paramSerie.Categories.Where(c => !categories.Select(x => x.Name.ToUpper()).Contains(c.ToUpper())).ToList();

            categories = categories.Concat(newCategories.Select(c => new Category(c))).ToList();

            var newSerie = new Serie
            (
                paramSerie.Title,
                paramSerie.Description,
                paramSerie.PosterImg,
                paramSerie.ReleasedDate,
                paramSerie.Seasons,
                categories
            );

            if (SerieExistsByKey(newSerie.SerieKey))
            {
                //return new BadRequestObjectResult($"{newSerie.SerieKey} já existe");
                var serieExist = await _context.Serie.Include(s => s.Seasons).ThenInclude(s => s.Episodes).FirstOrDefaultAsync(s => s.SerieKey == newSerie.SerieKey);
                var newSeason = new Season();
                foreach (var season in newSerie.Seasons)
                {
                    var existSeason = serieExist.Seasons.FirstOrDefault(s => s.SeasonKey.Equals(season.SeasonKey));
                    if (existSeason != null)
                    {
                        foreach (var episode in season.Episodes)
                        {
                            var newEpisode = existSeason.Episodes.FirstOrDefault(e => e.EpisodeKey.Equals(episode.EpisodeKey));
                            if (newEpisode == null)
                            {
                                var addEpisode = SaveImagesOfEpisode(episode);
                                serieExist.Seasons.First(s => s.SeasonKey == season.SeasonKey).Episodes.Add(addEpisode);
                                serieExist.LatestRelease = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        serieExist.Seasons.Add(season);
                        serieExist.LatestRelease = DateTime.Now;

                    }
                }
            }
            else
            {
                newSerie = SaveImagesOfSerie(newSerie);
                _context.Serie.Add(newSerie);
            }

            await _context.SaveChangesAsync();

            return new OkObjectResult("Cadastrado com Sucesso");
        }
        public async Task<ActionResult> PostSeries(List<ParamSerie> paramSeries)
        {
            List<string> seriesWithErro = new List<string>();
            foreach(var paramSerie in paramSeries)
            {
                try
                {
                    var categories = _context.Category.Where(i => paramSerie.Categories.Contains(i.Name.ToUpper())).ToList();

                    var newCategories = paramSerie.Categories.Where(c => !categories.Select(x => x.Name.ToUpper()).Contains(c.ToUpper())).ToList();

                    categories = categories.Concat(newCategories.Select(c => new Category(c))).ToList();

                    var newSerie = new Serie
                    (
                        paramSerie.Title,
                        paramSerie.Description,
                        paramSerie.PosterImg,
                        paramSerie.ReleasedDate,
                        paramSerie.Seasons,
                        categories
                    );

                    if (SerieExistsByKey(newSerie.SerieKey))
                    {
                        //return new BadRequestObjectResult($"{newSerie.SerieKey} já existe");
                        var serieExist = await _context.Serie.Include(s => s.Seasons).ThenInclude(s => s.Episodes).FirstOrDefaultAsync(s => s.SerieKey == newSerie.SerieKey);
                        var newSeason = new Season();
                        foreach (var season in newSerie.Seasons)
                        {
                            var existSeason = serieExist.Seasons.FirstOrDefault(s => s.SeasonKey.Equals(season.SeasonKey));
                            if (existSeason != null)
                            {
                                foreach (var episode in season.Episodes)
                                {
                                    var newEpisode = existSeason.Episodes.FirstOrDefault(e => e.EpisodeKey.Equals(episode.EpisodeKey));
                                    if (newEpisode == null)
                                    {
                                        var addEpisode = SaveImagesOfEpisode(episode);
                                        serieExist.Seasons.First(s => s.SeasonKey == season.SeasonKey).Episodes.Add(addEpisode);
                                    }
                                }
                            }
                            else
                            {
                                serieExist.Seasons.Add(season);
                            }
                        }
                    }
                    else
                    {
                        newSerie = SaveImagesOfSerie(newSerie);
                        _context.Serie.Add(newSerie);
                    }
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    seriesWithErro.Add(paramSerie.Title + ": (exception): " + ex.Message);
                }
            }
            if (seriesWithErro.Count > 0)
                return new OkObjectResult("Cadastrado com algumas excessões: " + string.Join(';', seriesWithErro));
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
        private Serie SaveImagesOfSerie(Serie serie)
        {
            serie.PosterImg = Utils.Download(serie.PosterImg, serie.SerieKey, _configuration["Directories:ImagesPath"]);
            int iS = 0;
            foreach (var season in serie.Seasons)
            {
                int iE = 0;
                foreach (var episode in season.Episodes)
                {
                    serie.Seasons[iS].Episodes[iE].EpisodeImg = Utils.Download(serie.Seasons[iS].Episodes[iE].EpisodeImg, serie.Seasons[iS].Episodes[iE].EpisodeKey, _configuration["Directories:ImagesPath"]);
                    iE++;
                }
                iS++;
            }
            return serie;
        }
        private Episode SaveImagesOfEpisode(Episode episode)
        {
            episode.EpisodeImg = Utils.Download(episode.EpisodeImg, episode.EpisodeKey, _configuration["Directories:ImagesPath"]);
            return episode;
        }
        private Serie GetImageUrlSerie(Serie serie)
        {
            serie.PosterImg = Utils.GetFileUrl(serie.PosterImg, _configuration["Directories:BaseUrl"], _configuration["Directories:ImagesPath"]);
            if (serie.Seasons is not null)
            {
                int isS = 0;
                foreach (var season in serie.Seasons)
                {
                    int iE = 0;
                    foreach (var episode in season.Episodes)
                    {
                        serie.Seasons[isS].Episodes[iE].EpisodeImg = Utils.GetFileUrl(serie.Seasons[isS].Episodes[iE].EpisodeImg, _configuration["Directories:BaseUrl"], _configuration["Directories:ImagesPath"]);
                        iE++;
                    }
                    isS++;
                }
            }
            return serie;
        }
    }
}
