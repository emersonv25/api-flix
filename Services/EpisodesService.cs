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
            var episode = await _context.Espisode
                .Include(s => s.Season)
                .ThenInclude(s => s.Serie)
                .Include(s => s.Season.Episodes)
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
        public async Task<ActionResult> PostEpisode (string serieKey, int seasonNum, ParamEpisode episode)
        {
            if(serieKey is null || seasonNum < 1 || episode is null)
            {
                return new BadRequestObjectResult("Preencha os parâmetros");
            }

            var season = _context.Season.Where(s => s.Serie.SerieKey == serieKey && s.SeasonNum == seasonNum)
                .Include(s => s.Episodes)
                .FirstOrDefault();

            if(season is null)
            {
                return new NotFoundObjectResult("Série/Temporada não encontrada");
            }
            if (season.Episodes.Any(e => e.EpisodeNum == episode.EpisodeNum))
            {
                return new BadRequestObjectResult($"O Episódio nº {episode.EpisodeNum} já existe");
            }
            var newEpisode = new Episode(episode, seasonNum, serieKey);
            newEpisode.SeasonId = season.SeasonId;
            _context.Espisode.Add(newEpisode);
            var serie = await _context.Serie.FirstOrDefaultAsync(s => s.SerieKey == serieKey);
            serie.LatestRelease = DateTime.Now;
            await _context.SaveChangesAsync();
            return new OkObjectResult("Cadastrado com Sucesso");
        }
        private string GetImageUrlEpisode(Episode episode, string baseUrl)
        {
            string imgUrl = Utils.GetFileUrl(episode.EpisodeImg, baseUrl, _configuration["Directories:ImagesPath"]);

            return imgUrl;
        }
    }
}
