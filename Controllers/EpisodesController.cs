using Api.MyFlix.Models.Object;
using Api.MyFlix.Services;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodesController : ControllerBase
    {
        private readonly IEpisodesService _episodeService;
        public EpisodesController(IEpisodesService episodesService)
        {
            _episodeService = episodesService;
        }

        // GET: api/Episode/one-piece-episodio-1
        [HttpGet("{key}")]
        public async Task<ActionResult<ReturnEpisode>> GetEpisodeByKey(string key)
        {
            return await _episodeService.GetEpisodeByKey(key);
        }

    }
}
