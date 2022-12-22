using MediatR;

namespace Questao5.Application.Notifications
{
    public class IdempotencyNotification : INotification
    {
        public IdempotencyNotification(string chaveIdempotencia, string requisicao, string resultado)
        {
            ChaveIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public string ChaveIdempotencia { get; }
        public string Requisicao { get; }
        public string Resultado { get; }
    }
}
