using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRental.Movie;
using MovieRental.Common;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieFeatures _features;

        public MovieController(IMovieFeatures features)
        {
            _features = features;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1)
                    return BadRequest(new ErrorResponse("Page number must be greater than 0"));

                if (pageSize < 1)
                    return BadRequest(new ErrorResponse("Page size must be greater than 0"));

                var result = await _features.GetAllAsync(page, pageSize);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to retrieve movies", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("An unexpected error occurred", ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Movie.Movie movie)
        {
            try
            {
                if (movie == null)
                    return BadRequest(new ErrorResponse("Movie data is required"));

                if (string.IsNullOrWhiteSpace(movie.Title))
                    return BadRequest(new ErrorResponse("Movie title is required"));

                var result = _features.Save(movie);
                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to save movie to database", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("An unexpected error occurred", ex.Message));
            }
        }
    }
}