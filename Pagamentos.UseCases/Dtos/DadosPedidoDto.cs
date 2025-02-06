using System.Text.Json.Serialization;

namespace Pagamentos.UseCases.Dtos
{
    public class DadosPedidoDto
    {
        [JsonIgnore]
        public string? IdPagamento { get; set; }
        public string IdPedido { get; set; }
        public double Valor { get; set; }
    }
}
