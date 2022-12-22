using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Account? GetByAccount(string numero);
        Account? GetById(string id);
    }
}
