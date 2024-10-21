using MassTransit;

namespace GrpcSubProject
{
    public static class ConfigurationExtensions
    {
        public static void AddMassTransitExtensions(this IServiceCollection service, IConfiguration configuration)
        {
            var host = configuration.GetSection("MQconfiguration:HostName").Value;

            service.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host);
                });
            });
        }
    }
}
