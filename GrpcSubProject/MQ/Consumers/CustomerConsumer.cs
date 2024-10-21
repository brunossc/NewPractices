using GrpcSubProject.Data.Repositories;
using GrpcSubProject.Model;
using MassTransit;
using NewPractices.MQ.Contracts;

namespace GrpcSubProject.MQ.Consumers
{
    public class CustomerConsumer : IConsumer<CustomerEvent>
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerConsumer> _logger;

        public CustomerConsumer(ICustomerRepository repository, ILogger<CustomerConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CustomerEvent> context)
        {
            var customer = new Customer()
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                Document = context.Message.Document
            };

            _logger.LogInformation("Received customer: {Id}, {Name}, {Document}", customer.Id, customer.Name, customer.Document);

            await _repository.AddAsync(customer);
        }
    }
}
