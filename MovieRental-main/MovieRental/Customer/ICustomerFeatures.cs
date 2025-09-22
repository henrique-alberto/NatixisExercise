namespace MovieRental.Customer;

public interface ICustomerFeatures
{
    Task<Customer> Save(Customer customer);
    Task<List<Customer>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<Customer?> GetByIdAsync(int id);
}