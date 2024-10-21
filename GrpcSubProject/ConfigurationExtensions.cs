using GrpcSubProject.MQ.Consumers;
using MassTransit;

namespace GrpcSubProject
{
    public static class ConfigurationExtensions
    {
        public static void AddMassTransitExtensions(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddMassTransit(x =>
            {
                x.AddConsumer<CustomerConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var host = configuration.GetSection("MQconfiguration:HostName").Value;
                    var queue = configuration.GetSection("MQconfiguration:QueueName").Value;

                    cfg.Host(host);
                    cfg.ReceiveEndpoint(queue, e =>
                    {
                        e.ConfigureConsumer<CustomerConsumer>(context);
                    });
                });
            });

            service.AddHostedService<MassTransitHostedService>();
        }
    }
}
