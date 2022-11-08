using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<ActionResult<IEnumerable<ReturnMovies>>> GetMovie();
        Task<ActionResult<ReturnMovie>> GetMovieById(int id);
        Task<ActionResult<ReturnMovie>> GetMovieByKey(string key);
        Task<ActionResult<IEnumerable<ReturnMovies>>> SearchMovie(string name);
        Task<ActionResult> PutMovie(int id, Movie movie);
        Task<ActionResult> PostMovie(ParamMovie movie);
        Task<ActionResult> DeleteMovie(int id);
    }
}
