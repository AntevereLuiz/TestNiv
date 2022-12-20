namespace Questao2.Models
{
    public class MatchPagined
    {
        public int Page { get; set; }
        public int Total_pages { get; set; }
        public List<Match> Data { get; set; } = null!;
    }
}
