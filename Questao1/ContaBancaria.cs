namespace Questao1
{
    class ContaBancaria
    {
        const double TAXA_SAQUE = 3.50;

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        //Só é possível atribuir o valor no constrututor.
        //Requisito: Após a conta ser aberta, o número da conta nunca poderá ser alterado. 
        public readonly int Numero;
        public string Titular { get; private set; }

        //private set para impedir que o saldo seja modificado livremente.
        //Requisito: O saldo da conta não pode ser alterado livremente. É preciso haver um mecanismo para proteger isso.
        public double Saldo { get; private set; }

        //Requisito: O saldo só aumenta por meio de depósitos. 
        public void Deposito(double quantia)
        {
            Saldo += quantia;
        }

        //Requisito: O saldo só diminui por meio de saques. 
        public void Saque(double quantia)
        {
            Saldo -= quantia;
            AplicarTaxaDeSaque();
        }

        //Método utilizado no método Saque.
        //Requisito: Para cada saque realizado, a instituição cobra uma taxa de $ 3.50.
        private void AplicarTaxaDeSaque()
        {
            Saldo -= TAXA_SAQUE;
        }

        //Possibilidade de alterar o nome do titular:
        //Requisito: Já o nome do titular pode ser alterado (pois uma pessoa pode mudar de nome quando contrai matrimônio por exemplo).
        public void AlterarNomeDoTitular(string nome)
        {
            Titular = nome;
        }
    }
}
