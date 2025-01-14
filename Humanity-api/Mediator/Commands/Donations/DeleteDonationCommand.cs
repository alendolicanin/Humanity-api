using MediatR;

namespace Humanity.API.Mediator.Commands.Donations
{
    public class DeleteDonationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
