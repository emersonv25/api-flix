using Api.MyFlix.Models.Object;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Services.Interfaces
{
    public interface IEpisodesService
    {
        Task<ActionResult<ReturnEpisode>> GetEpisodeByKey(string key);
    }
}
