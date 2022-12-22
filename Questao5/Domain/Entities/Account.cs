namespace Questao5.Domain.Entities
{
    public class Account
    {
        public string IdContaCorrente { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public int Ativo { get; set; }
    }
}
