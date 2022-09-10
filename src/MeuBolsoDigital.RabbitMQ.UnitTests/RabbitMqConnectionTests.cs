using Moq;
using Moq.AutoMock;
using RabbitMQ.Client;

namespace MeuBolsoDigital.RabbitMQ.UnitTests
{
    public class RabbitMqConnectionTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly IRabbitMqConnection _rabbitMqConnection;

        public RabbitMqConnectionTests()
        {
            _autoMocker = new AutoMocker();
            _rabbitMqConnection = _autoMocker.CreateInstance<RabbitMqConnection>();

            _autoMocker.GetMock<IConnectionFactory>()
                .Setup(x => x.CreateConnection())
                .Returns(_autoMocker.GetMock<IConnection>().Object);
        }

        [Fact]
        public void TryConnect_IsConnected_DoNothing()
        {
            // Arrange
            _autoMocker.GetMock<IConnection>()
                .Setup(x => x.IsOpen)
                .Returns(true);

            var cancellationToken = new CancellationToken();

            // Act
            _rabbitMqConnection.TryConnect(cancellationToken);
            _rabbitMqConnection.TryConnect(cancellationToken);

            // Assert
            _autoMocker.GetMock<IConnectionFactory>()
                .Verify(x => x.CreateConnection(), Times.Once);

            _autoMocker.GetMock<IConnection>()
                .Verify(x => x.CreateModel(), Times.Once);
        }

        [Fact]
        public void TryConnect_CancellationTokenIsRequested_DoNothing()
        {
            // Arrange
            _autoMocker.GetMock<IConnection>()
                .Setup(x => x.IsOpen)
                .Returns(true);

            var cancellationToken = new CancellationToken(true);

            // Act
            _rabbitMqConnection.TryConnect(cancellationToken);

            // Assert
            _autoMocker.GetMock<IConnectionFactory>()
                .Verify(x => x.CreateConnection(), Times.Never);

            _autoMocker.GetMock<IConnection>()
                .Verify(x => x.CreateModel(), Times.Never);
        }

        [Fact]
        public void NoConnect_IsConnect_ReturnFalse()
        {
            // Assert
            Assert.False(_rabbitMqConnection.IsConnected);
        }

        [Fact]
        public void HaveConnect_IsConnect_ReturnTrue()
        {
            // Arrange
            _autoMocker.GetMock<IConnection>()
                .Setup(x => x.IsOpen)
                .Returns(true);

            var cancellationToken = new CancellationToken();

            // Act
            _rabbitMqConnection.TryConnect(cancellationToken);

            // Assert
            Assert.True(_rabbitMqConnection.IsConnected);
        }

        [Fact]
        public void TryConnect_ReturnSuccess()
        {
            // Arrange
            _autoMocker.GetMock<IConnection>()
                .Setup(x => x.IsOpen)
                .Returns(true);

            _autoMocker.GetMock<IConnection>()
                .Setup(x => x.CreateModel())
                .Returns(_autoMocker.GetMock<IModel>().Object);

            var cancellationToken = new CancellationToken();

            // Act
            _rabbitMqConnection.TryConnect(cancellationToken);

            // Assert
            _autoMocker.GetMock<IConnectionFactory>()
                .Verify(x => x.CreateConnection(), Times.Once);

            _autoMocker.GetMock<IConnection>()
                .Verify(x => x.CreateModel(), Times.Once);

            Assert.NotNull(_rabbitMqConnection.Connection);
            Assert.NotNull(_rabbitMqConnection.Channel);
            Assert.True(_rabbitMqConnection.IsConnected);
        }
    }
}