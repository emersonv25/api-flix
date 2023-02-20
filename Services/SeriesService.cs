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
        private readonly IConfiguration _configuration;

        public SeriesService(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;

        }
        public async Task<ActionResult<Result>> GetSerie(string search, string keys,int currentPage, int pageSize, string orderBy, string sortOrder, string baseUrl)
        {
            #region pagination
            int count = 0;
            int skip = (currentPage - 1) * pageSize;
            int take = pageSize;
            #endregion

            #region sort
            string columnOrder = "Title";
            bool isAsc = sortOrder == "desc" ? false : true;
            switch (orderBy)
            {
                case "title":
                    columnOrder = "Title";
                    break;
                case "latest_release":
                    columnOrder = "LatestRelease";
                    break;
                case "created_date":
                    columnOrder = "CreatedDate";
                    break;
                case "released_date":
                    columnOrder = "ReleasedDate";
                    break;
                case "most_view":
                    columnOrder = "Views";
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
                    returnSeries.Add(new ReturnSeries(GetImageUrlSerie(serie, baseUrl)));
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
        public async Task<ActionResult<ReturnSerie>> GetSerieByKey(string key, string baseUrl)
        {
            var serie = await _context.Serie
                .Include(m => m.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes).ThenInclude(e => e.EpisodeVideos)
                .FirstOrDefaultAsync(m => m.SerieKey == key);
            
            if (serie is not null)
            {
                foreach(var season in serie.Seasons)
                {
                    season.Episodes = season.Episodes.OrderBy(e => e.EpisodeNum).ToList();
                }
                //serie.Views += 1;
                //_context.SaveChanges();
                return new ReturnSerie(GetImageUrlSerie(serie, baseUrl));
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult<string[]>> GetSerieKeyList()
        {
            var series = await _context.Serie.Select(s => s.SerieKey).ToArrayAsync();

            if (series is not null && series.Length > 0)
            {
                return series;
            }

            return new NotFoundResult();
        }
        public async Task<ActionResult> AddView(string key)
        {
            var serie = await _context.Serie.FirstOrDefaultAsync(m => m.SerieKey == key);

            if (serie is not null)
            {
                serie.Views += 1;
                _context.SaveChanges();
                return new OkResult();
            }

            return new NotFoundResult();
        }
        public async Task<ActionResult<ReturnSerie>> GetSerieByTitle(string title, string baseUrl)
        {
            var serie = await _context.Serie
                .Include(c => c.Categories)
                .Include(m => m.Seasons)
                .ThenInclude(s => s.Episodes)
                .ThenInclude(e => e.EpisodeVideos)
                .FirstOrDefaultAsync(m => m.Title == title);

            if (serie is not null)
            {
                return new ReturnSerie(GetImageUrlSerie(serie, baseUrl));
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
                paramSerie.Rating,
                paramSerie.Seasons,
                categories
            );

            if (SerieExistsByKey(newSerie.SerieKey))
            {
                //return new BadRequestObjectResult($"{newSerie.SerieKey} já existe");
                var serieExist = await _context.Serie.Include(s => s.Seasons).ThenInclude(s => s.Episodes).FirstOrDefaultAsync(s => s.SerieKey == newSerie.SerieKey);
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
                                var addEpisode = await SaveImagesOfEpisodeAsync(episode);
                                serieExist.Seasons.First(s => s.SeasonKey == season.SeasonKey).Episodes.Add(addEpisode);
                                serieExist.LatestRelease = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        for(var i = 0; i < season.Episodes.Count; i++)
                        {
                            season.Episodes[i] = await SaveImagesOfEpisodeAsync(season.Episodes[i]);
                        }
                        serieExist.Seasons.Add(season);
                        serieExist.LatestRelease = DateTime.Now;

                    }
                }
            }
            else
            {
                newSerie = await SaveImagesOfSerieAsync(newSerie);
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
                        paramSerie.Rating,
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
                                        var addEpisode = await SaveImagesOfEpisodeAsync(episode);
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
                        newSerie = await SaveImagesOfSerieAsync(newSerie);
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
        public async Task<ActionResult> PatchSerie(string serieKey, ParamSerieUpdate paramSerie)
        {

            var serieDb = await _context.Serie.Include(s => s.Categories).FirstOrDefaultAsync(s => s.SerieKey == serieKey);

            if (!SerieExistsByKey(serieKey))
            {
                return new NotFoundResult();
            }

            if(serieDb.Title != paramSerie.Title && !string.IsNullOrWhiteSpace(paramSerie.Title))
            {
                serieDb.Title = paramSerie.Title;
            }
            if(serieDb.Description != paramSerie.Description && !string.IsNullOrWhiteSpace(paramSerie.Description))
            {
                serieDb.Description = paramSerie.Description;
            }
            if(serieDb.PosterImg != paramSerie.PosterImg && !string.IsNullOrWhiteSpace(paramSerie.PosterImg))
            {
                serieDb.PosterImg= paramSerie.PosterImg;
                await SaveImageOfSeriePosterOnlyAsync(serieDb);
            }
            if(serieDb.Rating != paramSerie.Rating && !string.IsNullOrWhiteSpace(paramSerie.Rating))
            {
                serieDb.Rating = paramSerie.Rating;   
            }
            if(serieDb.ReleasedDate != paramSerie.ReleasedDate && !string.IsNullOrWhiteSpace(paramSerie.ReleasedDate))
            {
                serieDb.ReleasedDate = paramSerie.ReleasedDate;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BadRequestObjectResult("Não foi possível salvar as alterações no banco de dados");
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
        private async Task<Serie> SaveImageOfSeriePosterOnlyAsync(Serie serie)
        {
            serie.PosterImg = await Utils.Upload(serie.PosterImg, serie.SerieKey, _configuration["Directories:ImagesPath"]);
            return serie;
        }
        private async Task<Serie> SaveImagesOfSerieAsync(Serie serie)
        {
            serie.PosterImg = await Utils.Upload(serie.PosterImg, serie.SerieKey, _configuration["Directories:ImagesPath"]);
            int iS = 0;
            foreach (var season in serie.Seasons)
            {
                int iE = 0;
                foreach (var episode in season.Episodes)
                {
                    serie.Seasons[iS].Episodes[iE].EpisodeImg = await Utils.Upload(serie.Seasons[iS].Episodes[iE].EpisodeImg, serie.Seasons[iS].Episodes[iE].EpisodeKey, _configuration["Directories:ImagesPath"]);
                    iE++;
                }
                iS++;
            }
            return serie;
        }
        private async Task<Episode> SaveImagesOfEpisodeAsync(Episode episode)
        {
            episode.EpisodeImg = await Utils.Upload(episode.EpisodeImg, episode.EpisodeKey, _configuration["Directories:ImagesPath"]);
            return episode;
        }
        private Serie GetImageUrlSerie(Serie serie, string baseUrl)
        {
            serie.PosterImg = Utils.GetFileUrl(serie.PosterImg, baseUrl, _configuration["Directories:ImagesPath"]);
            if (serie.Seasons is not null)
            {
                int isS = 0;
                foreach (var season in serie.Seasons)
                {
                    int iE = 0;
                    foreach (var episode in season.Episodes)
                    {
                        serie.Seasons[isS].Episodes[iE].EpisodeImg = Utils.GetFileUrl(serie.Seasons[isS].Episodes[iE].EpisodeImg, baseUrl, _configuration["Directories:ImagesPath"]);
                        iE++;
                    }
                    isS++;
                }
            }
            return serie;
        }
    }
}
