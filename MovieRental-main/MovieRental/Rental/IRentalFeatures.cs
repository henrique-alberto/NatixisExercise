namespace MovieRental.Rental;

public interface IRentalFeatures
{
	Task<Rental> Save(Rental rental);
	Task<IEnumerable<Rental>> GetRentalsByCustomerId(string customerName);
}