using Moq;
using Pagamentos.Infrastructure.Gateways;
using Pagamentos.Infrastructure.RabbitMQ;
using Pagamentos.UseCases.Gateways;
using RabbitMQ.Client;

namespace Pagamentos.Infrastructure.Tests.Gateways
{
    public class RabbitMqPublisherGatewayTests
    {
        private IBrokerPublisherGateway _rabbitMqPublisherGateway;

        private Mock<IRabbitMqConnection> _rabbitMqConnectionMock;
        private Mock<IModel> _connectionModelMock;

        private const string PAGAMENTOS_EXCHANGE = "pedidosOperations";
        private const string MESSAGE_TOPIC = "pagamentoRealizado";

        [SetUp]
        public void Setup()
        {
            _connectionModelMock = new Mock<IModel>();
            _rabbitMqConnectionMock = new Mock<IRabbitMqConnection>();

            _connectionModelMock.Setup(x => x.ExchangeDeclare(It.IsAny<string>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<bool>(),
                                                              It.IsAny<bool>(),
                                                              It.IsAny<IDictionary<string, object>>()));

            _connectionModelMock.Setup(x => x.BasicPublish(It.IsAny<string>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<bool>(),
                                                           It.IsAny<IBasicProperties>(),
                                                           It.IsAny<ReadOnlyMemory<byte>>()));

            _rabbitMqConnectionMock.Setup(x => x.CreateChannel()).Returns(_connectionModelMock.Object);

            _rabbitMqPublisherGateway = new RabbitMqPublisherGateway(_rabbitMqConnectionMock.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_rabbitMqPublisherGateway, Is.Not.Null);
        }

        [Test]
        public void PublicarMensagem_Sends_Message_To_Pagamentos_Exchange_With_PagamentoRealizado_Topic()
        {
            _rabbitMqPublisherGateway.PublicarMensagem(new object());

            _connectionModelMock.Verify(x => x.BasicPublish(PAGAMENTOS_EXCHANGE,
                                                            MESSAGE_TOPIC,
                                                            It.IsAny<bool>(),
                                                            null,
                                                            It.IsAny<ReadOnlyMemory<byte>>()), Times.Once);
        }

        [Test]
        public void PublicarMensagem_Declare_A_Topic_Exchange()
        {
            _rabbitMqPublisherGateway.PublicarMensagem(new object());

            _connectionModelMock.Verify(x => x.ExchangeDeclare(PAGAMENTOS_EXCHANGE,
                                                               "topic",
                                                               It.IsAny<bool>(),
                                                               It.IsAny<bool>(),
                                                               It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}
