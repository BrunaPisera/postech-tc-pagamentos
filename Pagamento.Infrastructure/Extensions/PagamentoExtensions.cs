using Pagamentos.Core.Entidades;
using Pagamentos.Infrastructure.Models;

namespace Pagamentos.Infrastructure.Extensions
{
    public static class PagamentoExtensions
    {
        public static PagamentoAggregate ToAggregate(this Pagamento pagamento)
        {
            return new PagamentoAggregate()
            {
                Id = pagamento.Id,
                IdPedido = pagamento.IdPedido,
                Pago = pagamento.Pago.Value
            };
        }
    }
}
