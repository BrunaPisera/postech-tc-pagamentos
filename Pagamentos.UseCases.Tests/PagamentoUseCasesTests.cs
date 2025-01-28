using Moq;
using Pagamentos.Application.Exceptions;
using Pagamentos.Core.Entidades;
using Pagamentos.UseCases.Dtos;
using Pagamentos.UseCases.Gateways;
using Pagamentos.UseCases.Interfaces;

namespace Pagamentos.UseCases.Tests
{
    public class PagamentoUseCasesTests
    {
        private IPagamentoUseCases _pagamentoUseCases { get; set; }

        private Mock<IApiPagamentoGateway> _apiPagamentoGatewayMock;
        private Mock<IBrokerPublisherGateway> _brokerPublisherGatewayMock;
        private Mock<IPagamentoPersistanceGateway> _pagamentoPersistanceGatewayMock;

        private const string QR_CODE_DATA = "QR*CODE*DATA";

        [SetUp]
        public void Setup()
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

        [Test]
        public void Can_Create()
        {
            Assert.That(_pagamentoUseCases, Is.Not.Null);
        }

        #region ConfirmarPagamentoAsync

        [Test]
        public void ConfirmarPagamentoAsync_Throws_PagamentoNaoEncontradoException_When_Pagamento_Is_Not_Found()
        {
            _pagamentoPersistanceGatewayMock.Setup(x => x.GetPagamentoByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<PagamentoAggregate?>(null));

            Assert.That(async () => await _pagamentoUseCases.ConfirmarPagamentoAsync(Guid.NewGuid()), Throws.Exception.TypeOf<PagamentoNaoEncontradoException>());
        }

        [Test]
        public void ConfirmarPagamentoAsync_Does_Not_Throews_PagamentoNaoEncontradoException_When_Pagamento_Is_Found()
        {
            _pagamentoPersistanceGatewayMock.Setup(x => x.GetPagamentoByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<PagamentoAggregate?>(new PagamentoAggregate()));

            Assert.That(async () => await _pagamentoUseCases.ConfirmarPagamentoAsync(Guid.NewGuid()), Throws.Nothing);
        }

        [Test]
        public async Task ConfirmarPagamentoAsync_Changes_Payment_Status()
        {
            var pagamento = new PagamentoAggregate();

            _pagamentoPersistanceGatewayMock.Setup(x => x.GetPagamentoByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<PagamentoAggregate?>(pagamento));

            Assert.That(pagamento.Pago, Is.False);

            await _pagamentoUseCases.ConfirmarPagamentoAsync(Guid.NewGuid());

            Assert.That(pagamento.Pago, Is.True);
        }

        [Test]
        public async Task ConfirmarPagamentoAsync_Notifies_Other_Services_Through_Broker()
        {
            _pagamentoPersistanceGatewayMock.Setup(x => x.GetPagamentoByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<PagamentoAggregate?>(new PagamentoAggregate()));

            await _pagamentoUseCases.ConfirmarPagamentoAsync(Guid.NewGuid());

            _brokerPublisherGatewayMock.Verify(x => x.PublicarMensagem(It.IsAny<object>()), Times.Once());
        }
        #endregion

        #region GerarQrCodeParaPagamento
        [Test]
        public async Task GerarQrCodeParaPagamento_Saves_Pagamento_In_The_Database()
        {
            await _pagamentoUseCases.GerarQrCodeParaPagamento(new DadosPedidoDto()
            {
                IdPedido = Guid.NewGuid().ToString()
            });

            _pagamentoPersistanceGatewayMock.Verify(x => x.SavePagamentoAsync(It.IsAny<PagamentoAggregate>()), Times.Once);
        }

        [Test]
        public async Task GerarQrCodeParaPagamento_Calls_GerarQrCodeParaPagamentoAsync_And_Returns_Based_On_Its_Result()
        {
            var idPedido = Guid.NewGuid().ToString();
            var result = await _pagamentoUseCases.GerarQrCodeParaPagamento(new DadosPedidoDto()
            {
                IdPedido = idPedido
            });

            _apiPagamentoGatewayMock.Verify(x => x.GerarQrCodeParaPagamentoAsync(It.IsAny<DadosPedidoDto>()), Times.Once);

            Assert.That(result.IdPedido, Is.EqualTo(Guid.Parse(idPedido)));
            Assert.That(result.DadosQrCode, Is.EqualTo(QR_CODE_DATA));
        }
        #endregion
    }
}
