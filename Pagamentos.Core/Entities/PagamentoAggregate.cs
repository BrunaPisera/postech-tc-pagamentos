namespace Pagamentos.Core.Entidades
{
    public class PagamentoAggregate : Entity<Guid>, IAggregateRoot
    {
        public Guid IdPedido { get; set; }
        public bool Pago { get; set; } = false;
    }
}
