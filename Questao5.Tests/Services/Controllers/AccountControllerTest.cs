using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Notifications;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Services.Controllers;
using System.Web.Http.Results;
using Xunit;

namespace Questao5.Tests.Services.Controllers
{
    public class AccountControllerTest
    {
        private readonly Mock<IMediator> _mediator;

        public AccountControllerTest()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public void MakeTransactionAsync_MustReturnOK()
        {
            var requisicao = "123";

            _mediator.Setup(x => x.Send(It.IsAny<MakeTransactionCommand>(), default(CancellationToken))).Returns(Task.FromResult("123"));

            var controller = new AccountController(_mediator.Object);
            var result = controller.MakeTransactionAsync(It.IsAny<MakeTransactionCommand>()).Result;
            var okResult = result as OkObjectResult;

            _mediator.Verify(x => x.Send(It.IsAny<MakeTransactionCommand>(), default(CancellationToken)), Times.Once);
            Assert.Equal(200, okResult!.StatusCode);
            Assert.Equal(requisicao, okResult!.Value);
        }

        [Fact]
        public void MakeTransactionAsync_MustReturnBadRequest()
        {
            var erro = "erro";

            _mediator.Setup(x => x.Send(It.IsAny<MakeTransactionCommand>(), default(CancellationToken))).Throws(() => new Exception(erro));

            var controller = new AccountController(_mediator.Object);
            var result = controller.MakeTransactionAsync(It.IsAny<MakeTransactionCommand>()).Result;
            var badRequestResult = result as BadRequestObjectResult;

            _mediator.Verify(x => x.Send(It.IsAny<MakeTransactionCommand>(), default(CancellationToken)), Times.Once);
            Assert.Equal(400, badRequestResult!.StatusCode);
            Assert.Equal(erro, badRequestResult!.Value);
            Assert.ThrowsAsync<Exception>(() => controller.MakeTransactionAsync(It.IsAny<MakeTransactionCommand>()));
        }

        [Fact]
        public void GetBalanceAsync_MustReturnOK()
        {
            var numero = "123";
            var notification = new GetBalanceQuery() { Numero = numero };
            var response = new GetBalanceQueryResponse()
            {
                AccountNumber = numero,
                Balance = 100,
                OwnerAccount = "luiz",
                SearchDate = "12/12/2010 15:56"
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetBalanceQuery>(), default(CancellationToken))).Returns(Task.FromResult(response));

            var controller = new AccountController(_mediator.Object);
            var result = controller.GetBalanceAsync(It.IsAny<string>()).Result;
            var okResult = result as OkObjectResult;

            _mediator.Verify(x => x.Send(It.IsAny<GetBalanceQuery>(), default(CancellationToken)), Times.Once);
            Assert.Equal(200, okResult!.StatusCode);
            Assert.IsType<GetBalanceQueryResponse>(okResult.Value);
        }

        [Fact]
        public void GetBalanceAsync_MustReturnBadRequest()
        {
            var erro = "erro";

            _mediator.Setup(x => x.Send(It.IsAny<GetBalanceQuery>(), default(CancellationToken))).Throws(() => new Exception(erro));

            var controller = new AccountController(_mediator.Object);
            var result = controller.GetBalanceAsync(It.IsAny<string>()).Result;
            var badRequestResult = result as BadRequestObjectResult;

            _mediator.Verify(x => x.Send(It.IsAny<GetBalanceQuery>(), default(CancellationToken)), Times.Once);
            Assert.Equal(400, badRequestResult!.StatusCode);
            Assert.Equal(erro, badRequestResult!.Value);
            Assert.ThrowsAsync<Exception>(() => controller.GetBalanceAsync(It.IsAny<string>()));
        }
    }
}
