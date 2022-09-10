using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace MeuBolsoDigital.RabbitMQ
{
    public class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private readonly ILogger<RabbitMqConnection> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public IConnection Connection => _connection;
        public IModel Channel => _channel;
        public bool IsConnected => _connection?.IsOpen ?? false;

        public RabbitMqConnection(ILogger<RabbitMqConnection> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public void TryConnect(CancellationToken cancellationToken)
        {
            if (IsConnected || cancellationToken.IsCancellationRequested)
                return;

            var policy = Policy.Handle<ConnectFailureException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(5, retryAttemp =>
                {
                    _logger.LogInformation($"Attemp {retryAttemp}: connecting to RabbitMQ.");
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttemp));
                });

            policy.Execute(() =>
            {
                _logger.LogInformation("Connecting to RabbitMQ.");

                _connection = _connectionFactory.CreateConnection();
                _connection.ConnectionShutdown += OnDisconnect;
                _channel = _connection.CreateModel();

                _logger.LogInformation("Connected to RabbitMQ.");
            });
        }

        public virtual void OnDisconnect(object s, EventArgs e)
        {
            _logger.LogInformation("Disconnecting from RabbitMQ.");
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}