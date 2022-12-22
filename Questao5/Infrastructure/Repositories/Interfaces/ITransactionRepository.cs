using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<int> MakeTransactionAsync(Transaction transaction);
        Task<GetBalanceQueryResponse?> GetByBalanceAccountAsync(string numero);
    }
}
