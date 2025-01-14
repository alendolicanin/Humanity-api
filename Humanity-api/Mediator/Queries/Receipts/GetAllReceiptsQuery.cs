using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.Receipts
{
    public class GetAllReceiptsQuery : IRequest<IEnumerable<ReceiptDto>>
    {
    }
}
