using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Movie
{
	public class MovieFeatures : IMovieFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		private const int DefaultPageSize = 10;

		public MovieFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}
		
		public Movie Save(Movie movie)
		{
			_movieRentalDb.Movies.Add(movie);
			_movieRentalDb.SaveChanges();
			return movie;
		}

		public async Task<List<Movie>> GetAllAsync(int page = 1, int pageSize = DefaultPageSize)
		{
			try
			{
				return await _movieRentalDb.Movies
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to retrieve movies from database", ex);
			}
		}
	}
}