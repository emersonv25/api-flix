using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Services.Interfaces
{
    public interface ISeriesService
    {
        Task<ActionResult<IEnumerable<ReturnSeries>>> GetSerie();
        Task<ActionResult<ReturnSerie>> GetSerieById(int id);
        Task<ActionResult<ReturnSerie>> GetSerieByKey(string key);
        Task<ActionResult<IEnumerable<ReturnSeries>>> SearchSerie(string name);
        Task<ActionResult> PutSerie(int id, Serie Serie);
        Task<ActionResult> PostSerie(ParamSerie Serie);
        Task<ActionResult> DeleteSerie(int id);
    }
}
