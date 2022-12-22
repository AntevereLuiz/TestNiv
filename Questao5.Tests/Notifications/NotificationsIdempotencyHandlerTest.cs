using Moq;
using Questao5.Application.Notifications;
using Questao5.Application.Notifications.NotificationsHandler;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Questao5.Tests.Notifications
{
    public class NotificationsIdempotencyHandlerTest
    {
        private readonly Mock<IIdempotencyRepository> _idempotencyRepository;

        public NotificationsIdempotencyHandlerTest()
        {
            _idempotencyRepository = new Mock<IIdempotencyRepository>();
        }

        [Fact]
        public void NotificationsIdempotencyHandler_MustSendNotification()
        {
            var notification = new IdempotencyNotification(Guid.NewGuid().ToString(), "teste requisição", "Success");

            _idempotencyRepository.Setup(x => x.CreateIdempotencyKeyAsync(It.IsAny<Idempotency>())).ReturnsAsync(1);
            
            var handler = new NotificationsIdempotencyHandler(_idempotencyRepository.Object);
            handler.Handle(notification, new CancellationToken()).Wait();

            _idempotencyRepository.Verify(x => x.CreateIdempotencyKeyAsync(It.IsAny<Idempotency>()), Times.Once);
        }
    }
}
