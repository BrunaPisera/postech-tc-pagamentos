using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Pagamentos.Core.Entidades;
using Pagamentos.Infrastructure.Extensions;
using Pagamentos.Infrastructure.Models;
using Pagamentos.UseCases.Gateways;

namespace Pagamentos.Infrastructure.Gateways
{
    public class PagamentoPersistanceGateway : IPagamentoPersistanceGateway
    {
        private readonly IMongoCollection<Pagamento> _pagamentosCollection;

        public PagamentoPersistanceGateway(IConfiguration configuration)
        {
            var mongoDbConfig = configuration.GetSection("MongoDBConfig");
            var hostName = mongoDbConfig["HostName"];
            var userName = mongoDbConfig["UserName"];
            var password = mongoDbConfig["Password"];

            var mongoClient = new MongoClient($"mongodb://{userName}:{password}@{hostName}:27017/?authMechanism=SCRAM-SHA-256");

            var mongoDatabase = mongoClient.GetDatabase("PagamentosDB");

            _pagamentosCollection = mongoDatabase.GetCollection<Pagamento>("Pagamentos");
        }

        public async Task<PagamentoAggregate?> GetPagamentoByIdAsync(Guid idPagamento)
        {
            var result = await _pagamentosCollection.Find(x => x.Id == idPagamento).FirstOrDefaultAsync();

            return result?.ToAggregate();
        }

        public async Task SavePagamentoAsync(PagamentoAggregate pagamento) =>
            await _pagamentosCollection.InsertOneAsync(pagamento.ToPagamento());

        public async Task UpdatePagamentoAsync(PagamentoAggregate pagamento) =>
            await _pagamentosCollection.ReplaceOneAsync(x => x.Id == pagamento.Id, pagamento.ToPagamento());
    }
}
