using Humanity.API.Mediator.Queries.Receipts;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Receipts
{
    public class GetAllReceiptsQueryHandler : IRequestHandler<GetAllReceiptsQuery, IEnumerable<ReceiptDto>>
    {
        private readonly IReceiptService _receiptService;

        public GetAllReceiptsQueryHandler(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        public async Task<IEnumerable<ReceiptDto>> Handle(GetAllReceiptsQuery request, CancellationToken cancellationToken)
        {
            return await _receiptService.GetAllReceiptsAsync();
        }
    }
}
