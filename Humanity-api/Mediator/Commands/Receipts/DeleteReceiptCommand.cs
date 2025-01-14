using MediatR;

namespace Humanity.API.Mediator.Commands.Receipts
{
    public class DeleteReceiptCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
