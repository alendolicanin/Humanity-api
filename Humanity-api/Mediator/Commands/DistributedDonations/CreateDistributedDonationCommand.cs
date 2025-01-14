using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Commands.DistributedDonations
{
    public class CreateDistributedDonationCommand : IRequest<DistributedDonationDto>
    {
        public CreateDistributedDonationDto CreateDistributedDonationDto { get; set; }
    }
}
