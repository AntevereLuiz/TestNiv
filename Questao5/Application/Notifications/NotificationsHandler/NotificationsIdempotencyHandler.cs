using MediatR;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Application.Notifications.NotificationsHandler
{
    public class NotificationsIdempotencyHandler : INotificationHandler<IdempotencyNotification>
    {
        private readonly IIdempotencyRepository _idempotencyRepository;
        public NotificationsIdempotencyHandler(IIdempotencyRepository idempotencyRepository)
        {
            _idempotencyRepository = idempotencyRepository;
        }

        public async Task Handle(IdempotencyNotification notification, CancellationToken cancellationToken)
        {
            var idempotency = new Idempotency(notification.ChaveIdempotencia, notification.Requisicao, notification.Resultado);

            await _idempotencyRepository.CreateIdempotencyKeyAsync(idempotency);
        }
    }
}
