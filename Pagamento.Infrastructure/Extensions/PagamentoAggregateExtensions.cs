using Pagamentos.Core.Entidades;
using Pagamentos.Infrastructure.Models;

namespace Pagamentos.Infrastructure.Extensions
{
    public static class PagamentoAggregateExtensions
    {
        public static Pagamento ToPagamento(this PagamentoAggregate aggregate) {
            return new Pagamento()
            {
                Id = aggregate.Id,
                IdPedido = aggregate.IdPedido,
                Pago = aggregate.Pago
            };
        }
    }
}
