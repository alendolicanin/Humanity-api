using Humanity.Application.Interfaces;
using Humanity_api.Mediator.Commands.Receipts;
using MediatR;

namespace Humanity_api.Mediator.Handlers.Receipts
{
    public class ConfirmSignatureCommandHandler : IRequestHandler<ConfirmSignatureCommand, bool>
    {
        private readonly IReceiptService _receiptService;

        public ConfirmSignatureCommandHandler(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        public async Task<bool> Handle(ConfirmSignatureCommand request, CancellationToken cancellationToken)
        {
            return await _receiptService.ConfirmSignatureAsync(request.ReceiptId, request.RecipientId);
        }
    }
}
