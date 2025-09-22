using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Customer
{
    public class CustomerFeatures : ICustomerFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;
        private const int DefaultPageSize = 10;

        public CustomerFeatures(MovieRentalDbContext movieRentalDb)
        {
            _movieRentalDb = movieRentalDb;
        }

        public async Task<Customer> Save(Customer customer)
        {
            await _movieRentalDb.Customers.AddAsync(customer);
            await _movieRentalDb.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllAsync(int page = 1, int pageSize = DefaultPageSize)
        {
            try
            {
                return await _movieRentalDb.Customers
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve customers from database", ex);
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _movieRentalDb.Customers.FindAsync(id);
        }
    }
}