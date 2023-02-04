using Api.MyFlix.Models.Object;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Services.Interfaces
{
    public interface IEpisodesService
    {
        /// <summary>
        /// Get episode by episodeKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ActionResult<ReturnEpisode>> GetEpisodeByKey(string key, string baseUrl);
        /// <summary>
        /// Get last episodes uploaded 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        Task<ActionResult<Result>> GetLastEpisodes(int currentPage, int pageSize, string baseUrl);
        /// <summary>
        /// ADD a list of episodes
        /// </summary>
        /// <param name="serieKey"></param>
        /// <param name="seasonNum"></param>
        /// <param name="episodes"></param>
        /// <returns></returns>
        Task<ActionResult> PostEpisodes(string serieKey, int seasonNum, List<ParamEpisode> episodes);
        /// <summary>
        /// Delete episode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ActionResult> DeleteEpisode(int id);
        /// <summary>
        /// Add view in episode
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ActionResult> AddView(string key);
    }
}
