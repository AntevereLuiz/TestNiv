using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Repositories.Interfaces;
using System.Net;
using System.Web.Http;

namespace Questao5.Application.Handlers
{
    public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, GetBalanceQueryResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        const string INVALID_ACCOUNT = "Type: Invalid Account. Account not found";
        const string INACTIVE_ACCOUNT = "Type: Inactive Account. Inactive account";
        const string AND = "and";

        public GetBalanceQueryHandler(ITransactionRepository transactionRepository,
                                        IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }
        public async Task<GetBalanceQueryResponse> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            errors = Validations(request);
            if (errors.Any()) {
                throw new Exception(String.Join($" {AND} ", errors));
            }

            var balance = await _transactionRepository.GetByBalanceAccountAsync(request.Numero);
            return balance!;
        }

        private List<string> Validations(GetBalanceQuery request)
        {
            var errors = new List<string>();

            var account = _accountRepository.GetByAccount(request.Numero);

            if (account == null)
                errors.Add(INVALID_ACCOUNT);

            if (account?.Ativo == (int)StatusAccountEnum.Inactive)
                errors.Add(INACTIVE_ACCOUNT);

            return errors;
        }
    }
}
