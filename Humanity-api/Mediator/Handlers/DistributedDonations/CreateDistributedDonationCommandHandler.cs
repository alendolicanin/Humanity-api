using Humanity.API.Mediator.Commands.DistributedDonations;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.DistributedDonations
{
    public class CreateDistributedDonationCommandHandler : IRequestHandler<CreateDistributedDonationCommand, DistributedDonationDto>
    {
        private readonly IDistributedDonationService _distributedDonationService;

        public CreateDistributedDonationCommandHandler(IDistributedDonationService distributedDonationService)
        {
            _distributedDonationService = distributedDonationService;
        }

        public async Task<DistributedDonationDto> Handle(CreateDistributedDonationCommand request, CancellationToken cancellationToken)
        {
            return await _distributedDonationService.CreateDistributedDonationAsync(request.CreateDistributedDonationDto);
        }
    }
}
