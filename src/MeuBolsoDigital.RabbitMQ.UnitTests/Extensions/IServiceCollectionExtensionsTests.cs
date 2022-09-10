using MeuBolsoDigital.RabbitMQ.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using RabbitMQ.Client;

namespace MeuBolsoDigital.RabbitMQ.UnitTests.Extensions
{
    public class IServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddDependencyInjection_ReturnSuccess()
        {
            // Arrange
            var autoMock = new AutoMocker();
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddRabbitMqConnection(autoMock.GetMock<IConfiguration>().Object);

            // Assert
            var connectionFactory = serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(IConnectionFactory));
            Assert.NotNull(connectionFactory);
        }
    }
}