namespace MinimalApiCustomer.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer> GetByIdAsync(Guid id);
}