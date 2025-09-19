using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public RentalFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}

		// Made async: non-blocking DB operations
		public async Task<Rental> Save(Rental rental)
		{
			await _movieRentalDb.Rentals.AddAsync(rental);
			await _movieRentalDb.SaveChangesAsync();
			return rental;
		}

		//TODO: finish this method and create an endpoint for it
		public async Task<IEnumerable<Rental>> GetRentalsByCustomerName(string customerName)
		{
			return await _movieRentalDb.Rentals
				.Where(r => r.CustomerName == customerName)
				.ToListAsync();
		}

	}
}
