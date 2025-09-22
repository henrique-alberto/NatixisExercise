namespace MovieRental.Movie;

public interface IMovieFeatures
{
	Movie Save(Movie movie);
	Task<List<Movie>> GetAllAsync(int page = 1, int pageSize = 10);
}