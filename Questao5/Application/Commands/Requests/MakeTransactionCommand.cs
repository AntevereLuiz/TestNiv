using MediatR;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class MakeTransactionCommand : IRequest<string>
    {
        public string ChaveIdempotencia { get; set; } = null!;
        public string IdContaCorrente { get; set; } = null!;
        public decimal Valor { get; set; }
        public TypeTransactionEnum TipoMovimento { get; set; }

    }
}