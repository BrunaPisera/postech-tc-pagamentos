using Microsoft.AspNetCore.Mvc;
using Moq;
using Pagamentos.API.Controllers;
using Pagamentos.Infrastructure.Models.MercadoPagoAPI;
using Pagamentos.UseCases.Dtos;
using Pagamentos.UseCases.Interfaces;

namespace Pagamentos.API.Tests.Controllers
{
    public class PagamentoControllerTests
    {
        private PagamentoController _controller;

        private Mock<IPagamentoUseCases> _pagamentoUseCasesMock;

        private const string QR_CODE_DATA = "QR*CODE*DATA";

        [SetUp]
        public void Setup()
        {
            _pagamentoUseCasesMock = new Mock<IPagamentoUseCases>();

            _controller = new PagamentoController(_pagamentoUseCasesMock.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_controller, Is.Not.Null);
        }

        #region GerarPagamento
        [Test]
        public async Task GerarPagamento_Returns_BadRequest_When_Body_Is_Null()
        {
            var result = await _controller.GerarPagamento(null);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task GerarPagamento_Returns_BadRequest_When_IdPagamento_Is_Null_Or_Empty(string? idPagamento)
        {
            var result = await _controller.GerarPagamento(new DadosPedidoDto()
            {
                IdPedido = idPagamento
            });

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GerarPagamento_Returns_QrCode_Data()
        {
            var qrCodeReturn = new PedidoQrCodeDTO()
            {
                DadosQrCode = QR_CODE_DATA
            };


            _pagamentoUseCasesMock.Setup(x => x.GerarQrCodeParaPagamento(It.IsAny<DadosPedidoDto>()))
                .Returns(Task.FromResult(qrCodeReturn));

            var result = await _controller.GerarPagamento(new DadosPedidoDto()
            {
                IdPedido = Guid.NewGuid().ToString()
            });

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(((OkObjectResult) result).Value, Is.TypeOf<PedidoQrCodeDTO>());
            Assert.That(((PedidoQrCodeDTO)((OkObjectResult) result).Value).DadosQrCode, Is.EqualTo(QR_CODE_DATA));
        }
        #endregion

        #region ConfirmarPagamento
        [Test]
        public async Task ConfirmarPagamento_Returns_BadRequest_When_idPagamento_Is_Null()
        {
            var result = await _controller.ConfirmarPagamento(Guid.Empty, new MPNotificacaoDePagamento() { topic = "payment" });

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task ConfirmarPagamento_Returns_Ok_When_idPagamento_Is_Not_Null()
        {
            var result = await _controller.ConfirmarPagamento(Guid.NewGuid(), new MPNotificacaoDePagamento() { topic = "payment" });

            Assert.That(result, Is.TypeOf<OkResult>());
        }
        #endregion
    }
}
