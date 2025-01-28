using Pagamentos.UseCases.Dtos;

namespace Pagamentos.UseCases.Interfaces
{
    public interface IPagamentoUseCases
    {
        Task ConfirmarPagamentoAsync(Guid idPagamento);
        Task<PedidoQrCodeDTO> GerarQrCodeParaPagamento(DadosPedidoDto dadosPedidoDto);
    }
}
