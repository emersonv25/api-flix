using Api.MyFlix.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.MyFlix.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<ActionResult<IEnumerable<Movie>>> GetMovie();
        Task<ActionResult<Movie?>> GetMovie(int id);
        Task<ActionResult> PutMovie(int id, Movie movie);
        Task<ActionResult> PostMovie(Movie movie);
        Task<ActionResult> DeleteMovie(int id);
    }
}
