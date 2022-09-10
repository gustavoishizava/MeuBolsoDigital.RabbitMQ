using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MeuBolsoDigital.RabbitMQ.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = configuration["RabbitMqConfiguration:HostName"],
                UserName = configuration["RabbitMqConfiguration:UserName"],
                Password = configuration["RabbitMqConfiguration:Password"]
            };

            if (int.TryParse(configuration["RabbitMqConfiguration:Port"], out int port))
                connectionFactory.Port = port;

            var uri = configuration["RabbitMqConfiguration:Uri"];
            if (!string.IsNullOrEmpty(uri))
                connectionFactory.Uri = new Uri(uri);

            services.AddSingleton<IConnectionFactory>()
                    .AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

            return services;
        }
    }
}