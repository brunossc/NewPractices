using GrpcSubProject.Model;

namespace GrpcSubProject.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid id);
        Task AddAsync(Customer customer);
    }
}
