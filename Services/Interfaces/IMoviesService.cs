using Api.MyFlix.Models;
using Api.MyFlix.Models.Object;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<ActionResult<IEnumerable<ReturnMovies>>> GetMovie();
        Task<ActionResult<ReturnMovie>> GetMovie(int id);
        Task<ActionResult> PutMovie(int id, Movie movie);
        Task<ActionResult> PostMovie(Movie movie);
        Task<ActionResult> DeleteMovie(int id);
    }
}
