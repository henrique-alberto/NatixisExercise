using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		private readonly IPaymentProviderFactory _paymentProviderFactory;

		public RentalFeatures(MovieRentalDbContext movieRentalDb, IPaymentProviderFactory paymentProviderFactory)
		{
			_movieRentalDb = movieRentalDb;
			_paymentProviderFactory = paymentProviderFactory;
		}

		public async Task<Rental> Save(Rental rental)
		{
			// Process payment first
			var paymentProvider = _paymentProviderFactory.GetPaymentProvider(rental.PaymentMethod);
			// Assuming $10 per day rental fee - in real world this would come from Movie.Price * DaysRented
			var rentalPrice = rental.DaysRented * 10.0;
			var paymentResult = await paymentProvider.Pay(rentalPrice);

			if (!paymentResult)
			{
				throw new InvalidOperationException("Payment failed. Rental cannot be processed.");
			}

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