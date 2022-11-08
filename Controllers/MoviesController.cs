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
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnMovie>> GetMovie(int id)
        {
            return await _moviesService.GetMovie(id);
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
