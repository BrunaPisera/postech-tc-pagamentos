using Moq;
using Pagamentos.Core.Entidades;
using Pagamentos.UseCases;
using Pagamentos.UseCases.Dtos;
using Pagamentos.UseCases.Gateways;
using Pagamentos.UseCases.Interfaces;

namespace Pagamentos.BDD.Tests.StepDefinitions
{
    [Binding]
    public class PagamentoStepDefinitions
    {
        private DadosPedidoDto _dadosPedido;
        private IPagamentoUseCases _pagamentoUseCases { get; set; }

        private Mock<IApiPagamentoGateway> _apiPagamentoGatewayMock;
        private Mock<IBrokerPublisherGateway> _brokerPublisherGatewayMock;
        private Mock<IPagamentoPersistanceGateway> _pagamentoPersistanceGatewayMock;

        private const string QR_CODE_DATA = "QR*CODE*DATA";

        [BeforeScenario("pagamentos")]
        public void BeforeScenarioWithTag()
        {
            _apiPagamentoGatewayMock = new Mock<IApiPagamentoGateway>();
            _brokerPublisherGatewayMock = new Mock<IBrokerPublisherGateway>();
            _pagamentoPersistanceGatewayMock = new Mock<IPagamentoPersistanceGateway>();

            _pagamentoUseCases = new PagamentoUseCases(_apiPagamentoGatewayMock.Object,
                                                       _brokerPublisherGatewayMock.Object,
                                                       _pagamentoPersistanceGatewayMock.Object);

            _pagamentoPersistanceGatewayMock.Setup(x => x.UpdatePagamentoAsync(It.IsAny<PagamentoAggregate>()));
            _pagamentoPersistanceGatewayMock.Setup(x => x.SavePagamentoAsync(It.IsAny<PagamentoAggregate>()));

            _brokerPublisherGatewayMock.Setup(x => x.PublicarMensagem(It.IsAny<PagamentoRealizadoDto>()));

            _apiPagamentoGatewayMock.Setup(x => x.GerarQrCodeParaPagamentoAsync(It.IsAny<DadosPedidoDto>()))
                .Returns(Task.FromResult(QR_CODE_DATA));
        }

        [Given("an order of id {string}")]
        public void GivenAnOrderOfId(string p0)
        {
            _dadosPedido = new DadosPedidoDto()
            {
                IdPedido = p0
            };
        }

        [When("the application is called to generate a QR Code for payment")]
        public void WhenTheApplicationIsCalledToGenerateAQRCodeForPayment()
        {
            _pagamentoUseCases.GerarQrCodeParaPagamento(_dadosPedido);
        }

        [Then("the database is called to save the payment")]
        public void ThenTheDatabaseIsCalledToSaveThePayment()
        {
            _pagamentoPersistanceGatewayMock.Verify(x => x.SavePagamentoAsync(It.IsAny<PagamentoAggregate>()), Times.Once());
        }
    }
}
