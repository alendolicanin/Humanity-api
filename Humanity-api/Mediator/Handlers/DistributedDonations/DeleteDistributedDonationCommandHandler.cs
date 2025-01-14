using Humanity.API.Mediator.Commands.DistributedDonations;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.DistributedDonations
{
    public class DeleteDistributedDonationCommandHandler : IRequestHandler<DeleteDistributedDonationCommand, bool>
    {
        private readonly IDistributedDonationService _distributedDonationService;

        public DeleteDistributedDonationCommandHandler(IDistributedDonationService distributedDonationService)
        {
            _distributedDonationService = distributedDonationService;
        }

        public async Task<bool> Handle(DeleteDistributedDonationCommand request, CancellationToken cancellationToken)
        {
            return await _distributedDonationService.DeleteDistributedDonationAsync(request.Id);
        }
    }
}
