using Grpc.Core;
using GrpcSubProject.Data.Repositories;
using GrpcSubProject.Protos;

namespace GrpcSubProject.Services
{

    public class CustomerService : CustomerProtoService.CustomerProtoServiceBase
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _repository;
        public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger)
        {
            _logger = logger;
            _repository = repository;
        }
        public override async Task<CustomerReply> GetCustomerById(CustomerRequest request, ServerCallContext context)
        {
            var customer = await _repository.GetByIdAsync(Guid.Parse(request.Id));

            if (customer == null)
            {
                _logger.LogInformation($"Customer not found{request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            return new CustomerReply
            {
                Id = customer.Id.ToString(),
                Name = customer.Name,
                Document = customer.Document
            };
        }
    }
}