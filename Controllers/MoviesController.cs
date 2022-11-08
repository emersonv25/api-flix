using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.MyFlix.Data;
using Api.MyFlix.Models;
using Api.MyFlix.Services.Interfaces;
using Api.MyFlix.Models.Object;
using System.Xml.Linq;

namespace Api.MyFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnMovies>>> GetMovie()
        {
            return await _moviesService.GetMovie();
        }

        // GET: api/Movies/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReturnMovie>> GetMovieById(int id)
        {
            return await _moviesService.GetMovieById(id);
        }
        // GET: api/Movies/one-piece
        [HttpGet("{key}")]
        public async Task<ActionResult<ReturnMovie>> GetMovieByKey(string key)
        {
            return await _moviesService.GetMovieByKey(key);
        }

        // GET: api/Movies/search?name=One Piece
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ReturnMovies>>> SearchMovie(string name)
        {
            return await _moviesService.SearchMovie(name);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            return await _moviesService.PutMovie(id, movie);
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(ParamMovie movie)
        {
            return await _moviesService.PostMovie(movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            return await _moviesService.DeleteMovie(id);    
        }
    }
}
