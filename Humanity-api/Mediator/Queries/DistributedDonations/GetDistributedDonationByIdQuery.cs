using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.DistributedDonations
{
    public class GetDistributedDonationByIdQuery : IRequest<DistributedDonationDto>
    {
        public int Id { get; set; }
    }
}
