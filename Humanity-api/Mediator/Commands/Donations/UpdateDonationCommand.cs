using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Commands.Donations
{
    public class UpdateDonationCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public UpdateDonationDto UpdateDonationDto { get; set; }
    }
}
