using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Notifications;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Repositories.Interfaces;
using System.Text.Json;

namespace Questao5.Application.Handlers
{
    public class MakeTransactionCommandHandler : IRequestHandler<MakeTransactionCommand, string>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IIdempotencyRepository _idempotencyRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMediator _mediator;

        const string INVALID_ACCOUNT = "Type: Invalid Account. Account not found";
        const string INACTIVE_ACCOUNT = "Type: Inactive Account. Inactive account";
        const string INVALID_VALUE = "Type: Invalid value. The value must be greater than zero";
        const string INVALID_TYPE = "Type: Invalid type. The type must be 0 (credit) or 1 (debit)";
        const string TRANSACTION_ALREADY_DONE = "Transaction already done with";
        const string SUCCESS = "Success";
        const string AND = "and";

        public MakeTransactionCommandHandler(ITransactionRepository transactionRepository,
                                      IIdempotencyRepository idempotencyRepository,
                                      IAccountRepository accountRepository,
                                      IMediator mediator)
        {
            _transactionRepository = transactionRepository;
            _idempotencyRepository = idempotencyRepository;
            _accountRepository = accountRepository;
            _mediator = mediator;
        }
        public async Task<string> Handle(MakeTransactionCommand request, CancellationToken cancellationToken)
        {
            var transactionId = Guid.NewGuid().ToString();

            var transactionAlreadyDone = _idempotencyRepository.GetById(request.ChaveIdempotencia);
            if (transactionAlreadyDone != null)
                throw new Exception($"{TRANSACTION_ALREADY_DONE} {transactionAlreadyDone.Resultado}!");

            var errors = new List<string>();
            errors = Validations(request);
            if (errors.Any())
                throw new Exception(String.Join($" {AND} ", errors));

            ValidateValues(request);

            var transaction = new Domain.Entities.Transaction(transactionId, request.IdContaCorrente, DateTimeOffset.Now.ToString(),
                              char.Parse(request.TipoMovimento.ToString()), request.Valor);

            await _transactionRepository.MakeTransactionAsync(transaction);
            SendResultNotificationAsync(request, SUCCESS);

            return transactionId;
        }

        private void ValidateValues(MakeTransactionCommand request)
        {
            if (request.TipoMovimento == TypeTransactionEnum.C)
                request.Valor = Math.Abs(request.Valor);
            else
                request.Valor = Math.Abs(request.Valor) * -1;
        }

        private async void SendResultNotificationAsync(MakeTransactionCommand request, string msg)
        {
            var idempotencyidempotency = new IdempotencyNotification(request.ChaveIdempotencia, 
                                         JsonSerializer.Serialize(request), msg);
            await _mediator.Publish(idempotencyidempotency);
        }

        private List<string> Validations(MakeTransactionCommand request)
        {
            var errors = new List<string>();

            var account = _accountRepository.GetById(request.IdContaCorrente);

            if (account == null)
                errors.Add(INVALID_ACCOUNT);

            if (account?.Ativo == (int)StatusAccountEnum.Inactive)
                errors.Add(INACTIVE_ACCOUNT);

            if (request.Valor <= 0)
                errors.Add(INVALID_VALUE);

            if (request.TipoMovimento != TypeTransactionEnum.C && request.TipoMovimento != TypeTransactionEnum.D)
                errors.Add(INVALID_TYPE);

            return errors;
        }
    }
}
