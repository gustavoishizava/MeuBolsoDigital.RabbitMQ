using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MeuBolsoDigital.RabbitMQ.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionFactory>(new ConnectionFactory
            {
                HostName = configuration["RabbitMqConfiguration:HostName"],
                UserName = configuration["RabbitMqConfiguration:UserName"],
                Password = configuration["RabbitMqConfiguration:Password"],
                Port = int.Parse(configuration["RabbitMqConfiguration:Port"])
            });

            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

            return services;
        }
    }
}