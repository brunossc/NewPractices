using GrpcSubProject;
using GrpcSubProject.Data;
using GrpcSubProject.Data.Repositories;
using GrpcSubProject.Services;
using Microsoft.EntityFrameworkCore;
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
    options.UseInMemoryDatabase("CustomersDb"));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddGrpc();
builder.Services.AddMassTransitExtensions(builder.Configuration);

var app = builder.Build();


app.MapGrpcService<CustomerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

try
{
    Log.Information("Iniciando a aplicação");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
