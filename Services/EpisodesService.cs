using Api.MyFlix.Data;
using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Api.MyFlix.Services
{
    public class EpisodesService : IEpisodesService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public EpisodesService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ActionResult<ReturnEpisode>> GetEpisodeByKey(string key, string baseUrl)
        {
            var episode = await _context.Episode
                .Include(s => s.Season)
                .ThenInclude(s => s.Serie)
                .Include(s => s.Season.Episodes)
                .ThenInclude(e => e.EpisodeVideos)
                .FirstOrDefaultAsync(m => m.EpisodeKey == key);

            if (episode is not null)
            {
                var returnEpisode = new ReturnEpisode(episode);
                returnEpisode.EpisodeImg = GetImageUrlEpisode(episode, baseUrl);
                returnEpisode.SerieKey = episode.Season.Serie.SerieKey;
                returnEpisode.SeasonKey = episode.Season.SeasonKey;
                if(episode.Season.Episodes.Any(e => e.EpisodeNum == episode.EpisodeNum + 1))
                {
                    returnEpisode.NextEpisodeKey = episode.Season.Episodes
                        .Where(e => e.EpisodeNum == episode.EpisodeNum + 1)
                        .FirstOrDefault().EpisodeKey;
                }
                if (episode.Season.Episodes.Any(e => e.EpisodeNum == episode.EpisodeNum - 1))
                {
                    returnEpisode.PreviousEpisodeKey = episode.Season.Episodes
                        .Where(e => e.EpisodeNum == episode.EpisodeNum - 1)
                        .FirstOrDefault().EpisodeKey;
                }
                episode.Views += 1;
                _context.SaveChanges();
                return returnEpisode;
            }
            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
        public async Task<ActionResult> PostEpisodes(string serieKey, int seasonNum, List<ParamEpisode> episodes)
        {
            if (serieKey is null || seasonNum < 1 || episodes is null || episodes.Count == 0)
            {
                return new BadRequestObjectResult("Preencha os parâmetros");
            }

            var serie = await _context.Serie.Include(s => s.Seasons).FirstOrDefaultAsync(s => s.SerieKey == serieKey);
            if(serie is null)
            {
                return new NotFoundObjectResult("Série não encontrada");
            }

            var season = _context.Season.Where(s => s.Serie.SerieKey == serieKey && s.SeasonNum == seasonNum)
                .Include(s => s.Episodes)
                .FirstOrDefault();

            if (season is null)
            {
                var newSeason = new Season(new ParamSeason { SeasonNum = seasonNum, Episodes = episodes }, serieKey);
                for (var i = 0; i < newSeason.Episodes.Count; i++)
                {
                    newSeason.Episodes[i] = await SaveImagesOfEpisodeAsync(newSeason.Episodes[i]);
                }
                serie.Seasons.Add(newSeason);
            }
            else
            {
                foreach (var episode in episodes)
                {
                    if (season.Episodes.Any(e => e.EpisodeNum == episode.EpisodeNum))
                    {
                        return new BadRequestObjectResult($"O Episódio nº {episode.EpisodeNum} já existe");
                    }
                    var newEpisode = new Episode(episode, seasonNum, serieKey);
                    newEpisode.SeasonId = season.SeasonId;
                    if (!EpisodeExistsByKey(newEpisode.EpisodeKey))
                    {
                        newEpisode = await SaveImagesOfEpisodeAsync(newEpisode);
                        _context.Episode.Add(newEpisode);
                    }
                }
            }

            serie.LatestRelease = DateTime.Now;

            await _context.SaveChangesAsync();
            return new OkObjectResult("Cadastrado com Sucesso");
        }
        private async Task<Episode> SaveImagesOfEpisodeAsync(Episode episode)
        {
            episode.EpisodeImg = await Utils.Upload(episode.EpisodeImg, episode.EpisodeKey, _configuration["Directories:ImagesPath"]);
            return episode;
        }
        public async Task<ActionResult> DeleteEpisode(int id)
        {
            var episode = await _context.Episode.FindAsync(id);
            if (episode is null)
            {
                return new NotFoundObjectResult("Não encontrado");
            }

            _context.Episode.Remove(episode);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Deletado com sucesso");
        }
        private string GetImageUrlEpisode(Episode episode, string baseUrl)
        {
            string imgUrl = Utils.GetFileUrl(episode.EpisodeImg, baseUrl, _configuration["Directories:ImagesPath"]);

            return imgUrl;
        }
        private bool EpisodeExistsByKey(string key)
        {
            return _context.Episode.Any(e => e.EpisodeKey == key);
        }
    }
}
