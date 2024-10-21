using GrpcSubProject;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiCustomer.Data;
using MinimalApiCustomer.Repositories;
using NewPractices.MQ.Contracts;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
                 .ReadFrom.Services(services)
                 .WriteTo.Console());

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseInMemoryDatabase("CustomerDb"));


builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddMassTransitExtensions(builder.Configuration);

var app = builder.Build();

app.MapPost("/customers", async ([FromBody] Customer customer, [FromServices] ICustomerRepository repository, [FromServices] IBus bus) =>
{
    var isValid = await ValidateCustomerAsync(customer);

    if (isValid)
    {
        try
        {
            await repository.AddAsync(customer);

            var _event = new CustomerEvent()
            {
                Id = customer.Id,
                Name = customer.Name,
                Document = customer.Document,
            };

            await bus.Publish(_event);

            return Results.Ok("Customer added successfully and event published.");
        }
        catch (Exception ex)
        { }
    }

    return Results.BadRequest("Invalid customer data.");
});

app.Run();

async Task<bool> ValidateCustomerAsync(Customer customer)
{
    try
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync("http://customerfunc:7071/api/ValidateCustomers", customer);

        return response.IsSuccessStatusCode && await response.Content.ReadAsStringAsync() == "true";
    }
    catch (Exception ex)
    {
        return false;
    }
}

public record Customer(Guid Id, string Name, string Document);
