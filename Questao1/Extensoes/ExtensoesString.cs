namespace Questao1.Extensoes
{
    public static class ExtensoesString
    {
        public static string ToMoney(this double valor) {
            return $"$ {valor.ToString("N2")}";
        }
    }
}
