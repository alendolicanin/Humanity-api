using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.Donations
{
    public class GetAllDonationsQuery : IRequest<IEnumerable<DonationDto>>
    {
        public DonationQueryDto QueryDto { get; set; }
    }
}
