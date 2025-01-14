using Humanity.API.Mediator.Queries.Receipts;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Receipts
{
    public class GetReceiptByIdQueryHandler : IRequestHandler<GetReceiptByIdQuery, ReceiptDto>
    {
        private readonly IReceiptService _receiptService;

        public GetReceiptByIdQueryHandler(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        public async Task<ReceiptDto> Handle(GetReceiptByIdQuery request, CancellationToken cancellationToken)
        {
            return await _receiptService.GetReceiptByIdAsync(request.Id);
        }
    }
}
