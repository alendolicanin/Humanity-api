using MediatR;

namespace Humanity_api.Mediator.Commands.Receipts
{
    public class ConfirmSignatureCommand : IRequest<bool>
    {
        public int ReceiptId { get; set; }
        public string RecipientId { get; set; }
    }
}
