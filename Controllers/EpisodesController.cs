using Api.MyFlix.Models.Object;
using Api.MyFlix.Services.Interfaces;
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
            var baseUrl = string.Concat(Request.Scheme, "://", Request.Host.ToUriComponent());
            return await _episodeService.GetEpisodeByKey(key, baseUrl);
        }
        // GET: api/Episode/Last?currentPage=1&pageSize=25
        [HttpGet("Last")]
        public async Task<ActionResult<Result>> GetLastEpisodes(int currentPage = 1, int pageSize = 20)
        {
            var baseUrl = string.Concat(Request.Scheme, "://", Request.Host.ToUriComponent());
            return await _episodeService.GetLastEpisodes(currentPage, pageSize, baseUrl);
        }
        // POST: api/Episode
        [HttpPost()]
        public async Task<ActionResult> PostEpisodes(string serieKey, int seasonNum, List<ParamEpisode> episodes)
        {
            return await _episodeService.PostEpisodes(serieKey, seasonNum, episodes);
        }
        // DELETE: api/Episode
        [HttpDelete()]
        public async Task<ActionResult> DeleteEpisode(int id)
        {
            return await _episodeService.DeleteEpisode(id);
        }

    }
}
