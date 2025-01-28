using Pagamentos.UseCases.Dtos;

namespace Pagamentos.UseCases.Gateways
{
    public interface IApiPagamentoGateway
    {
        Task<string> GerarQrCodeParaPagamentoAsync(DadosPedidoDto dadosPedidoDto);
    }
}
