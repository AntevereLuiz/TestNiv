using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Repositories.Interfaces
{
    public interface IIdempotencyRepository
    {
        Task<int> CreateIdempotencyKeyAsync(Idempotency transaction);
        Idempotency? GetById(string id);
    }
}
