using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CustomerFunc
{
    public class ValidateCustomer
    {
        private readonly ILogger<ValidateCustomer> _logger;

        public ValidateCustomer(ILogger<ValidateCustomer> logger)
        {
            _logger = logger;
        }

        [Function("ValidateCustomers")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {

            _logger.LogInformation("Processing customer validation request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(requestBody);

            if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.Document))
            {
                return new BadRequestObjectResult("Invalid customer data.");
            }

            return new OkObjectResult("true");
        }

        public class Customer
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Document { get; set; }
        }
    }
}
