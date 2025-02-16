using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Pagamentos.Infrastructure.RabbitMQ
{
    public interface IRabbitMqConnection
    {
        IModel CreateChannel();
        void Dispose();
    }

    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnection _connection;

        public RabbitMqConnection(IConfiguration configuration)
        {
            var hostName = configuration["BROKER_HOSTNAME"];
            var port = int.Parse(configuration["BROKER_PORT"]!);
            var userName = configuration["BROKER_USERNAME"];
            var password = configuration["BROKER_PASSWORD"];
            var virtualHost = Environment.GetEnvironmentVariable("BROKER_VIRTUALHOST");

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost
            };

            _connection = factory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
