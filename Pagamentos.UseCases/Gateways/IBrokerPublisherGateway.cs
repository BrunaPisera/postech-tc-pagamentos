namespace Pagamentos.UseCases.Gateways
{
    public interface IBrokerPublisherGateway
    {
        void PublicarMensagem(object mensagem);
    }
}
