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
            var episodes = await _context.Espisode.FirstOrDefaultAsync(m => m.EpisodeKey == key);

            if (episodes is not null)
            {
                return new ReturnEpisode(episodes);
            }

            return new NotFoundObjectResult("Nenhum resultado encontrado");
        }
    }
}
