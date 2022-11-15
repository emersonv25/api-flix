using Api.MyFlix.Data;
using Api.MyFlix.Models.Object;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.MyFlix.Services
{
    public class EpisodesService : IEpisodesService
    {
        private readonly AppDbContext _context;

        public EpisodesService(AppDbContext context)
        {
            _context = context;

        }

        public async Task<ActionResult<ReturnEpisode>> GetEpisodeByKey(string key)
        {
            var episode = await _context.Espisode
                .Include(s => s.Season)
                .ThenInclude(s => s.Serie)
                .Include(s => s.Season.Episodes)
                .FirstOrDefaultAsync(m => m.EpisodeKey == key);

            if (episode is not null)
            {
                var returnEpisode = new ReturnEpisode(episode);
                returnEpisode.SerieKey = episode.Season.Serie.SerieKey;
                returnEpisode.SeasonKey = episode.Season.SeasonKey;
                if(episode.Season.Episodes.Any(e => e.EpisodeNum == episode.EpisodeNum + 1))
                {
                    returnEpisode.NextEpisodeKey = episode.Season.Episodes
                        .Where(e => e.EpisodeNum == episode.EpisodeNum + 1)
                        .ToList()[0].EpisodeKey;
                }
                if (episode.Season.Episodes.Any(e => e.EpisodeNum == episode.EpisodeNum - 1))
                {
                    returnEpisode.PreviousEpisodeKey = episode.Season.Episodes
                        .Where(e => e.EpisodeNum == episode.EpisodeNum - 1)
                        .ToList()[0].EpisodeKey;
                }

                return returnEpisode;
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
    }
}
