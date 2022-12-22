using MediatR;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Application.Notifications;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Questao5.Tests.Application.Handlers
{
    public class MakeTransactionCommandHandlerTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Mock<IIdempotencyRepository> _idempotencyRepository;
        private readonly Mock<IMediator> _mediator;
        public MakeTransactionCommandHandlerTest()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _accountRepository = new Mock<IAccountRepository>();
            _idempotencyRepository = new Mock<IIdempotencyRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public void Handle_MustReturnTransactionId()
        {
            var accountResponse = new Account()
            {
                Ativo = 1,
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Luiz",
                Numero = "123"
            };
            var idempotency = new Idempotency(Guid.NewGuid().ToString(), "requisição", "resultado");
            var makeTransactionCommand = new MakeTransactionCommand()
            {
                ChaveIdempotencia = Guid.NewGuid().ToString(),
                IdContaCorrente = Guid.NewGuid().ToString(),
                TipoMovimento = TypeTransactionEnum.D,
                Valor = 145.52M
            };

            _idempotencyRepository.Setup(x => x.GetById(It.IsAny<string>()));
            _accountRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(accountResponse);
            _transactionRepository.Setup(x => x.MakeTransactionAsync(It.IsAny<Transaction>())).ReturnsAsync(1);
            _mediator.Setup(x => x.Publish(It.IsAny<IdempotencyNotification>(), default(CancellationToken)));

            var handler = new MakeTransactionCommandHandler(_transactionRepository.Object, 
                                                            _idempotencyRepository.Object,
                                                            _accountRepository.Object,
                                                            _mediator.Object);
            var result = handler.Handle(makeTransactionCommand, default(CancellationToken)).Result;

            _idempotencyRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _accountRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _transactionRepository.Verify(x => x.MakeTransactionAsync(It.IsAny<Transaction>()), Times.Once);
            _mediator.Verify(x => x.Publish(It.IsAny<IdempotencyNotification>(), default(CancellationToken)), Times.Once);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Handle_MustThowAException_WhenFindIdempotencyKey()
        {
            var accountResponse = new Account()
            {
                Ativo = 1,
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Luiz",
                Numero = "123"
            };
            var idempotency = new Idempotency(Guid.NewGuid().ToString(), "requisição", "resultado");
            var makeTransactionCommand = new MakeTransactionCommand()
            {
                ChaveIdempotencia = Guid.NewGuid().ToString(),
                IdContaCorrente = Guid.NewGuid().ToString(),
                TipoMovimento = TypeTransactionEnum.D,
                Valor = 145.52M
            };

            _idempotencyRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(idempotency);
            _accountRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(accountResponse);
            _transactionRepository.Setup(x => x.MakeTransactionAsync(It.IsAny<Transaction>())).ReturnsAsync(1);
            _mediator.Setup(x => x.Publish(It.IsAny<IdempotencyNotification>(), default(CancellationToken)));

            var handler = new MakeTransactionCommandHandler(_transactionRepository.Object,
                                                            _idempotencyRepository.Object,
                                                            _accountRepository.Object,
                                                            _mediator.Object);

            Assert.ThrowsAsync<Exception>(() => handler.Handle(makeTransactionCommand, default(CancellationToken)));

            _idempotencyRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _accountRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Never);
            _transactionRepository.Verify(x => x.MakeTransactionAsync(It.IsAny<Transaction>()), Times.Never);
            _mediator.Verify(x => x.Publish(It.IsAny<Transaction>(), default(CancellationToken)), Times.Never);
        }

        [Fact]
        public void Handle_MustThowAException_WhenAccountIsInvalid()
        {
            var idempotency = new Idempotency(Guid.NewGuid().ToString(), "requisição", "resultado");
            var makeTransactionCommand = new MakeTransactionCommand()
            {
                ChaveIdempotencia = Guid.NewGuid().ToString(),
                IdContaCorrente = Guid.NewGuid().ToString(),
                TipoMovimento = TypeTransactionEnum.D,
                Valor = 145.52M
            };

            _idempotencyRepository.Setup(x => x.GetById(It.IsAny<string>()));
            _accountRepository.Setup(x => x.GetById(It.IsAny<string>()));
            _transactionRepository.Setup(x => x.MakeTransactionAsync(It.IsAny<Transaction>())).ReturnsAsync(1);
            _mediator.Setup(x => x.Publish(It.IsAny<IdempotencyNotification>(), default(CancellationToken)));

            var handler = new MakeTransactionCommandHandler(_transactionRepository.Object,
                                                            _idempotencyRepository.Object,
                                                            _accountRepository.Object,
                                                            _mediator.Object);

            Assert.ThrowsAsync<Exception>(() => handler.Handle(makeTransactionCommand, default(CancellationToken)));

            _idempotencyRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _accountRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _transactionRepository.Verify(x => x.MakeTransactionAsync(It.IsAny<Transaction>()), Times.Never);
            _mediator.Verify(x => x.Publish(It.IsAny<Transaction>(), default(CancellationToken)), Times.Never);
        }

        [Fact]
        public void Handle_MustThowAException_WhenAccountIsInactive()
        {
            var accountResponse = new Account()
            {
                Ativo = 0,
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Luiz",
                Numero = "123"
            };
            var idempotency = new Idempotency(Guid.NewGuid().ToString(), "requisição", "resultado");
            var makeTransactionCommand = new MakeTransactionCommand()
            {
                ChaveIdempotencia = Guid.NewGuid().ToString(),
                IdContaCorrente = Guid.NewGuid().ToString(),
                TipoMovimento = TypeTransactionEnum.D,
                Valor = 145.52M
            };

            _idempotencyRepository.Setup(x => x.GetById(It.IsAny<string>()));
            _accountRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(accountResponse);
            _transactionRepository.Setup(x => x.MakeTransactionAsync(It.IsAny<Transaction>())).ReturnsAsync(1);
            _mediator.Setup(x => x.Publish(It.IsAny<IdempotencyNotification>(), default(CancellationToken)));

            var handler = new MakeTransactionCommandHandler(_transactionRepository.Object,
                                                            _idempotencyRepository.Object,
                                                            _accountRepository.Object,
                                                            _mediator.Object);

            Assert.ThrowsAsync<Exception>(() => handler.Handle(makeTransactionCommand, default(CancellationToken)));

            _idempotencyRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _accountRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _transactionRepository.Verify(x => x.MakeTransactionAsync(It.IsAny<Transaction>()), Times.Never);
            _mediator.Verify(x => x.Publish(It.IsAny<Transaction>(), default(CancellationToken)), Times.Never);
        }

        [Fact]
        public void Handle_MustThowAException_WhenValueIsLessThanZero()
        {
            var accountResponse = new Account()
            {
                Ativo = 1,
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Luiz",
                Numero = "123"
            };
            var idempotency = new Idempotency(Guid.NewGuid().ToString(), "requisição", "resultado");
            var makeTransactionCommand = new MakeTransactionCommand()
            {
                ChaveIdempotencia = Guid.NewGuid().ToString(),
                IdContaCorrente = Guid.NewGuid().ToString(),
                TipoMovimento = TypeTransactionEnum.D,
                Valor = -120
            };

            _idempotencyRepository.Setup(x => x.GetById(It.IsAny<string>()));
            _accountRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(accountResponse);
            _transactionRepository.Setup(x => x.MakeTransactionAsync(It.IsAny<Transaction>())).ReturnsAsync(1);
            _mediator.Setup(x => x.Publish(It.IsAny<IdempotencyNotification>(), default(CancellationToken)));

            var handler = new MakeTransactionCommandHandler(_transactionRepository.Object,
                                                            _idempotencyRepository.Object,
                                                            _accountRepository.Object,
                                                            _mediator.Object);

            Assert.ThrowsAsync<Exception>(() => handler.Handle(makeTransactionCommand, default(CancellationToken)));

            _idempotencyRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _accountRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
            _transactionRepository.Verify(x => x.MakeTransactionAsync(It.IsAny<Transaction>()), Times.Never);
            _mediator.Verify(x => x.Publish(It.IsAny<Transaction>(), default(CancellationToken)), Times.Never);
        }
    }
}
