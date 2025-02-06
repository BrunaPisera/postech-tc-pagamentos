using Pagamentos.Application.Exceptions;
using Pagamentos.Core.Entidades;
using Pagamentos.UseCases.Dtos;
using Pagamentos.UseCases.Gateways;
using Pagamentos.UseCases.Interfaces;

namespace Pagamentos.UseCases
{
    public class PagamentoUseCases : IPagamentoUseCases
    {
        private readonly IApiPagamentoGateway ApiPagamentoGateway;
        private readonly IBrokerPublisherGateway BrokerPublisherGateway;
        private readonly IPagamentoPersistanceGateway PagamentoPersistanceGateway;

        public PagamentoUseCases(IApiPagamentoGateway apiPagamentoGateway,
                                 IBrokerPublisherGateway brokerPublisherGateway,
                                 IPagamentoPersistanceGateway pagamentoPersistanceGateway)
        {
            ApiPagamentoGateway = apiPagamentoGateway;
            BrokerPublisherGateway = brokerPublisherGateway;
            PagamentoPersistanceGateway = pagamentoPersistanceGateway;
        }

        public async Task ConfirmarPagamentoAsync(Guid idPagamento)
        {
            var pagamento = await PagamentoPersistanceGateway.GetPagamentoByIdAsync(idPagamento);

            if (pagamento == null) throw new PagamentoNaoEncontradoException("Pagamento não encontrado");

            pagamento.Pago = true;

            await PagamentoPersistanceGateway.UpdatePagamentoAsync(pagamento);

            BrokerPublisherGateway.PublicarMensagem(new PagamentoRealizadoDto()
            {
                IdPedido = pagamento.IdPedido
            });
        }

        public async Task<PedidoQrCodeDTO> GerarQrCodeParaPagamento(DadosPedidoDto dadosPedidoDto)
        {
            var pagamento = new PagamentoAggregate()
            {
                Id = Guid.NewGuid(),
                Pago = false,
                IdPedido = Guid.Parse(dadosPedidoDto.IdPedido)
            };

            await PagamentoPersistanceGateway.SavePagamentoAsync(pagamento);

            dadosPedidoDto.IdPagamento = pagamento.Id.ToString();

            var dadosQrCode = await ApiPagamentoGateway.GerarQrCodeParaPagamentoAsync(dadosPedidoDto!);

            var qrCodeInfo = new PedidoQrCodeDTO()
            {
                IdPedido = Guid.Parse(dadosPedidoDto.IdPedido),
                DadosQrCode = dadosQrCode
            };

            return qrCodeInfo;
        }
    }
}
