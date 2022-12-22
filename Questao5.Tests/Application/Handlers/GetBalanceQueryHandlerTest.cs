using Moq;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Questao5.Tests.Application.Handlers
{
    public class GetBalanceQueryHandlerTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<IAccountRepository> _accountRepository;

        public GetBalanceQueryHandlerTest()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _accountRepository = new Mock<IAccountRepository>();
        }

        [Fact]
        public void Handle_MustReturnGetBalanceQueryResponse()
        {
            var balanceQuery = new GetBalanceQuery()
            {
                Numero = "123"
            };
            var balanceResponse = new GetBalanceQueryResponse()
            {
                AccountNumber = "123",
                Balance = 100,
                OwnerAccount = "Luiz",
                SearchDate = "12/11/1987 12:04"
            };
            var accountResponse = new Account()
            {
                Ativo = 1,
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Luiz",
                Numero = "123"
            };

            _transactionRepository.Setup(x => x.GetByBalanceAccountAsync(It.IsAny<string>())).ReturnsAsync(balanceResponse);
            _accountRepository.Setup(x => x.GetByAccount(It.IsAny<string>())).Returns(accountResponse);

            var handler = new GetBalanceQueryHandler(_transactionRepository.Object, _accountRepository.Object);
            var result = handler.Handle(balanceQuery, default(CancellationToken)).Result;

            _transactionRepository.Verify(x => x.GetByBalanceAccountAsync(It.IsAny<string>()), Times.Once);
            _accountRepository.Verify(x => x.GetByAccount(It.IsAny<string>()), Times.Once);
            Assert.Equal(result, balanceResponse);
        }

        [Fact]
        public void Handle_MustThrowAException_WhenAccountNotFound()
        {
            var balanceQuery = new GetBalanceQuery()
            {
                Numero = "123"
            };
            var balanceResponse = new GetBalanceQueryResponse()
            {
                AccountNumber = "123",
                Balance = 100,
                OwnerAccount = "Luiz",
                SearchDate = "12/11/1987 12:04"
            };

            _transactionRepository.Setup(x => x.GetByBalanceAccountAsync(It.IsAny<string>())).ReturnsAsync(balanceResponse);
            _accountRepository.Setup(x => x.GetByAccount(It.IsAny<string>()));

            var handler = new GetBalanceQueryHandler(_transactionRepository.Object, _accountRepository.Object);

            Assert.ThrowsAsync<Exception>(() => handler.Handle(balanceQuery, default(CancellationToken)));
            _transactionRepository.Verify(x => x.GetByBalanceAccountAsync(It.IsAny<string>()), Times.Never);
            _accountRepository.Verify(x => x.GetByAccount(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Handle_MustThrowAException_WhenAccountIsInactive()
        {
            var balanceQuery = new GetBalanceQuery()
            {
                Numero = "123"
            };
            var balanceResponse = new GetBalanceQueryResponse()
            {
                AccountNumber = "123",
                Balance = 100,
                OwnerAccount = "Luiz",
                SearchDate = "12/11/1987 12:04"
            };
            var accountResponse = new Account()
            {
                Ativo = 0,
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Luiz",
                Numero = "123"
            };

            _transactionRepository.Setup(x => x.GetByBalanceAccountAsync(It.IsAny<string>())).ReturnsAsync(balanceResponse);
            _accountRepository.Setup(x => x.GetByAccount(It.IsAny<string>())).Returns(accountResponse); ;

            var handler = new GetBalanceQueryHandler(_transactionRepository.Object, _accountRepository.Object);

            Assert.ThrowsAsync<Exception>(() => handler.Handle(balanceQuery, default(CancellationToken)));
            _transactionRepository.Verify(x => x.GetByBalanceAccountAsync(It.IsAny<string>()), Times.Never);
            _accountRepository.Verify(x => x.GetByAccount(It.IsAny<string>()), Times.Once);
        }
    }
}
