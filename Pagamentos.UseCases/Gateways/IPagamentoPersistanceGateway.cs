using Pagamentos.Core.Entidades;

namespace Pagamentos.UseCases.Gateways
{
    public interface IPagamentoPersistanceGateway
    {
        Task SavePagamentoAsync(PagamentoAggregate pagamento);
        Task UpdatePagamentoAsync(PagamentoAggregate pagamento);
        Task<PagamentoAggregate?> GetPagamentoByIdAsync(Guid idPagamento);
    }
}
