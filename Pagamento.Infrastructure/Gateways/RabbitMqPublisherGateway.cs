using Pagamentos.Infrastructure.RabbitMQ;
using Pagamentos.UseCases.Gateways;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Pagamentos.Infrastructure.Gateways
{
    public class RabbitMqPublisherGateway : IBrokerPublisherGateway
    {
        IRabbitMqConnection _rabbitMqConnection;

        public RabbitMqPublisherGateway(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void PublicarMensagem(object mensagem)
        {
            using (var channel = _rabbitMqConnection.CreateChannel())
            {
                channel.ExchangeDeclare(exchange: "pedidosOperations",
                                         type: "topic",
                                         durable: true,
                                         autoDelete: false);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));

                channel.BasicPublish(exchange: "pedidosOperations",
                                     routingKey: "pagamentoRealizado",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
