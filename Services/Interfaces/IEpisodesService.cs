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
        Task<ActionResult<ReturnEpisode>> GetEpisodeByKey(string key);
        /// <summary>
        /// ADD a new episode
        /// </summary>
        /// <param name="serieKey"></param>
        /// <param name="seasonNum"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        Task<ActionResult> PostEpisode(string serieKey, int seasonNum, ParamEpisode episode);
    }
}
