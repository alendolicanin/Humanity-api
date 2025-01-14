using Humanity.API.Mediator.Commands.Receipts;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Receipts
{
    public class DeleteReceiptCommandHandler : IRequestHandler<DeleteReceiptCommand, bool>
    {
        private readonly IReceiptService _receiptService;

        public DeleteReceiptCommandHandler(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        public async Task<bool> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
        {
            return await _receiptService.DeleteReceiptAsync(request.Id);
        }
    }
}
