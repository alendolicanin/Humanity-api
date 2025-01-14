using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.Receipts
{
    public class GetReceiptByIdQuery : IRequest<ReceiptDto>
    {
        public int Id { get; set; }
    }
}
