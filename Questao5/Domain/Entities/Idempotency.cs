namespace Questao5.Domain.Entities
{
    public class Idempotency
    {
        public Idempotency(string chave_Idempotencia, string requisicao, string resultado)
        {
            Chave_Idempotencia = chave_Idempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public string Chave_Idempotencia { get; }
        public string Requisicao { get; }
        public string Resultado { get; }
    }
}