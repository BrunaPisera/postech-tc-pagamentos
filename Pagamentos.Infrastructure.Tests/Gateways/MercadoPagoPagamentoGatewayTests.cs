using Microsoft.Extensions.Configuration;
using Moq;
using Pagamentos.Infrastructure.Gateways;

namespace Pagamentos.Infrastructure.Tests.Gateways
{
    public class MercadoPagoPagamentoGatewayTests
    {
        private MercadoPagoPagamentoGateway _mercadoPagoPagamentoGateway;
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void SetUp()
        {
            _configurationMock = new Mock<IConfiguration>();
            _mercadoPagoPagamentoGateway = new MercadoPagoPagamentoGateway(_configurationMock.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_mercadoPagoPagamentoGateway, Is.Not.Null);
        }
    }
}
