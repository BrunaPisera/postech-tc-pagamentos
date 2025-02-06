using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Pagamentos.Infrastructure.Models
{
    public class Pagamento
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid IdPedido { get; set; }
        public bool? Pago { get; set; }
    }
}
