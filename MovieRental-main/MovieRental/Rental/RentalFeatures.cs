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

		public async Task<Rental> Save(Rental rental)
		{
            if (rental.Customer != null)
            {
                var existingCustomer = await _movieRentalDb.Customers.FirstOrDefaultAsync(c => c.Name == rental.CustomerName);
                if (existingCustomer != null)
                {
                    _movieRentalDb.Entry(existingCustomer).CurrentValues.SetValues(rental.Customer);
                    rental.Customer = existingCustomer;
                }
            }

            if (rental.Movie != null)
            {
                var existingMovie = await _movieRentalDb.Movies.FindAsync(rental.MovieId);
                if (existingMovie != null)
                {
                    _movieRentalDb.Entry(existingMovie).State = EntityState.Unchanged;
                    rental.Movie = existingMovie;
                }
            }

            await _movieRentalDb.Rentals.AddAsync(rental);
            await _movieRentalDb.SaveChangesAsync();
            return rental;
        }

		public async Task<IEnumerable<Rental>> GetRentalsByCustomerName(string customerName)
		{
			return await _movieRentalDb.Rentals
				.Include(r => r.Customer)
				.Include(r => r.Movie)
				.Where(r => r.CustomerName == customerName)
				.ToListAsync();
		}
	}
}