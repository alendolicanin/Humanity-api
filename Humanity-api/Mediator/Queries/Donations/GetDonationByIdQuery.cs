using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.Donations
{
    public class GetDonationByIdQuery : IRequest<DonationDto>
    {
        public int Id { get; set; }
    }
}
