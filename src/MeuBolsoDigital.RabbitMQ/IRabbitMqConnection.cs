using RabbitMQ.Client;

namespace MeuBolsoDigital.RabbitMQ
{
    public interface IRabbitMqConnection
    {
        IConnection Connection { get; }
        IModel Channel { get; }
        bool IsConnected { get; }
        void TryConnect(CancellationToken cancellationToken);
        void OnDisconnect(object s, EventArgs e);
    }
}