using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.DistributedDonations
{
    public class GetAllDistributedDonationsQuery : IRequest<IEnumerable<DistributedDonationDto>>
    {
    }
}
