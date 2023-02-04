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
        /// <param name="search"></param>
        /// <param name="keys"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        Task<ActionResult<Result>> GetSerie(string search, string keys, int currentPage, int pageSize, string orderBy, string sortOrder, string baseUrl);
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
        Task<ActionResult<ReturnSerie>> GetSerieByKey(string key, string baseUrl);
        /// <summary>
        /// Get serie by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<ActionResult<ReturnSerie>> GetSerieByTitle(string title, string baseUrl);
        /// <summary>
        /// Update a serie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        Task<ActionResult> PutSerie(int id, Serie serie);
        /// <summary>
        /// Update some information of one serie by seriekey
        /// </summary>
        /// <param name="serieKey"></param>
        /// <param name="paramSerie"></param>
        /// <returns></returns>
        Task<ActionResult> PatchSerie(string serieKey, ParamSerieUpdate paramSerie);
        /// <summary>
        /// Add one serie
        /// </summary>
        /// <param name="paramSerie"></param>
        /// <returns></returns>
        Task<ActionResult> PostSerie(ParamSerie paramSerie);
        /// <summary>
        /// Add list of serie
        /// </summary>
        /// <param name="paramSeries"></param>
        /// <returns></returns>
        Task<ActionResult> PostSeries(List<ParamSerie> paramSeries);
        /// <summary>
        /// Delete serie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ActionResult> DeleteSerie(int id);
        /// <summary>
        /// Add view in serie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ActionResult> AddView(string key);
    }
}
