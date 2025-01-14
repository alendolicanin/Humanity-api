using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Commands.Donations
{
    public class CreateDonationCommand : IRequest<DonationDto>
    {
        public CreateDonationDto CreateDonationDto { get; set; }
    }
}
