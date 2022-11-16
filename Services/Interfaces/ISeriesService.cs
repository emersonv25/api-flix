using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Services.Interfaces
{
    public interface ISeriesService
    {
        /// <summary>
        /// Get all series
        /// </summary>
        /// <returns></returns>
        Task<ActionResult<IEnumerable<ReturnSeries>>> GetSerie();
        /// <summary>
        /// Get serie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ActionResult<ReturnSerie>> GetSerieById(int id);
        /// <summary>
        /// Get serie by seriKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ActionResult<ReturnSerie>> GetSerieByKey(string key);
        /// <summary>
        /// Search a serie by one string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ActionResult<IEnumerable<ReturnSeries>>> SearchSerie(string name);
        /// <summary>
        /// Update a serie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Serie"></param>
        /// <returns></returns>
        Task<ActionResult> PutSerie(int id, Serie Serie);
        /// <summary>
        /// Add one serie
        /// </summary>
        /// <param name="Serie"></param>
        /// <returns></returns>
        Task<ActionResult> PostSerie(ParamSerie Serie);
        /// <summary>
        /// Delete serie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ActionResult> DeleteSerie(int id);
    }
}
