using MediatR;

namespace Humanity.API.Mediator.Commands.DistributedDonations
{
    public class DeleteDistributedDonationCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
