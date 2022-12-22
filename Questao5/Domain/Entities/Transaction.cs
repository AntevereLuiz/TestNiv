namespace Questao5.Domain.Entities
{
    public class Transaction
    {
        public Transaction(string idMovimento, string idContaCorrente, string dataMovimento, 
                           char tipoMovimento, decimal valor)
        {
            IdMovimento = idMovimento;
            IdContaCorrente = idContaCorrente;
            DataMovimento = dataMovimento;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }

        public string IdMovimento { get; }
        public string IdContaCorrente { get; }
        public string DataMovimento { get; }
        public char TipoMovimento { get; }
        public decimal Valor { get; }
    }
}
