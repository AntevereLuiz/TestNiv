namespace Questao5.Application.Queries.Responses
{
    public class GetBalanceQueryResponse
    {
        public string? AccountNumber { get; set; }
        public string? OwnerAccount { get; set; }
        public string? SearchDate { get; set; }
        public decimal Balance { get; set; }
    }
}
