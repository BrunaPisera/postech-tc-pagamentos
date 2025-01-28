using Pagamentos.Infrastructure.RabbitMQ;
using Pagamentos.Infrastructure.Gateways;
using Pagamentos.UseCases;
using Pagamentos.UseCases.Gateways;
using Pagamentos.UseCases.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure (this IServiceCollection services)
        {
            services.AddScoped<IPagamentoUseCases, PagamentoUseCases>();
            services.AddScoped<IPagamentoPersistanceGateway, PagamentoPersistanceGateway>();
            services.AddScoped<IApiPagamentoGateway, MercadoPagoPagamentoGateway>();

            services.AddScoped<IBrokerPublisherGateway, RabbitMqPublisherGateway>();
            services.AddScoped<IRabbitMqConnection, RabbitMqConnection>();

            return services;
        }
    }
}