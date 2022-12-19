namespace Questao1.Extensions
{
    public static class ExtensionToInt
    {
        public static int ToInt(this string valor) {
            int numero;
            Int32.TryParse(valor, out numero);
            return numero;
        }
    }
}
