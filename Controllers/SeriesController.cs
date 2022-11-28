using Microsoft.AspNetCore.Mvc;
using Api.MyFlix.Models;
using Api.MyFlix.Services.Interfaces;
using Api.MyFlix.Models.Object;


namespace Api.MyFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesService _seriesService;
        public SeriesController(ISeriesService seriesService)
        {
            _seriesService = seriesService;
        }

        // GET: api/Series
        [HttpGet]
        public async Task<ActionResult<Result>> GetSerie([FromQuery] string search = null,[FromQuery] int currentPage = 1, [FromQuery] int pageSize = 15, [FromQuery] string sortOrder = "title")
        {
            return await _seriesService.GetSerie(search, currentPage, pageSize, sortOrder);
        }

        // GET: api/Series/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReturnSerie>> GetSerieById(int id)
        {
            return await _seriesService.GetSerieById(id);
        }

        // GET: api/Series/one-piece
        [HttpGet("{key}")]
        public async Task<ActionResult<ReturnSerie>> GetSerieByKey(string key)
        {
            return await _seriesService.GetSerieByKey(key);
        }

        // PUT: api/Series/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSerie(int id, Serie Serie)
        {
            return await _seriesService.PutSerie(id, Serie);
        }

        // POST: api/Series
        [HttpPost]
        public async Task<ActionResult<Serie>> PostSerie(ParamSerie Serie)
        {
            return await _seriesService.PostSerie(Serie);
        }

        // DELETE: api/Series/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSerie(int id)
        {
            return await _seriesService.DeleteSerie(id);    
        }
    }
}
