using Pagamentos.Infrastructure.Models.MercadoPagoAPI;
using Pagamentos.UseCases.Gateways;
using System.Globalization;
using System.Text.Json;
using Pagamentos.UseCases.Dtos;
using Microsoft.Extensions.Configuration;

namespace Pagamentos.Infrastructure.Gateways
{
    public class MercadoPagoPagamentoGateway : IApiPagamentoGateway
    {
        private const int PAYMENT_TIMEOUT_MINUTES = 15;

        public IConfigurationSection Configuration { get; }

        public MercadoPagoPagamentoGateway(IConfiguration configuration)
        {
            Configuration = configuration.GetSection("MercadoPagoAPI");
        }

        public async Task<string> GerarQrCodeParaPagamentoAsync(DadosPedidoDto dadosPedidoDto)
        {
            var payload = JsonSerializer.Serialize(GerarPayloadMercadoPago(dadosPedidoDto));

            MercadoPagoPedidoResponse? mpResponse = await EnviarRequisicao(payload);

            var qrCodeResponse = new MercadoPagoPagamentoViewModel()
            {
                QrCodeData = mpResponse?.qr_data
            };

            return qrCodeResponse.QrCodeData ?? "";
        }

        private async Task<MercadoPagoPedidoResponse?> EnviarRequisicao(string payload)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.mercadopago.com/instore/orders/qr/seller/collectors/{Configuration["USERID"]}/pos/{Configuration["POS_ID"]}/qrs");

            var token = Configuration["ACCESS_KEY"];
            request.Headers.Add("Authorization", $"Bearer {token}");

            request.Content = new StringContent(payload);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<MercadoPagoPedidoResponse>(await response.Content.ReadAsStringAsync());
        }

        private MercadoPagoPayload GerarPayloadMercadoPago(DadosPedidoDto dadosPedidoDto)
        {
            return new MercadoPagoPayload()
            {
                external_reference = dadosPedidoDto.IdPedido,
                title = "Pedido " + dadosPedidoDto.IdPedido,
                description = "Pedido na lanchonete.",
                total_amount = dadosPedidoDto.Valor,
                expiration_date = DateTime.UtcNow.AddMinutes(PAYMENT_TIMEOUT_MINUTES).ToString("yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture),
                items = new List<MercadoPagoItem>()
                {
                    new MercadoPagoItem()
                    {
                        category = "Pedido",
                            title = "Pedido " + dadosPedidoDto.IdPedido,
                            unit_price = dadosPedidoDto.Valor,
                            quantity = 1,
                            total_amount = dadosPedidoDto.Valor,
                            sponsor = new MercadoPagoSponsor()
                            {
                                id = long.Parse(Configuration["USERID"] ?? "0")
                            }
                    }
                },
                notification_url = $"{Configuration["NOTIFICATION_URL"]}/api/v1/Pagamento/{dadosPedidoDto.IdPagamento}/confirmarPagamento"
            };
        }
    }
}
