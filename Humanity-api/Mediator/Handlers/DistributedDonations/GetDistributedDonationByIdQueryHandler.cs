using Humanity.API.Mediator.Queries.DistributedDonations;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.DistributedDonations
{
    public class GetDistributedDonationByIdQueryHandler : IRequestHandler<GetDistributedDonationByIdQuery, DistributedDonationDto>
    {
        private readonly IDistributedDonationService _distributedDonationService;

        public GetDistributedDonationByIdQueryHandler(IDistributedDonationService distributedDonationService)
        {
            _distributedDonationService = distributedDonationService;
        }

        public async Task<DistributedDonationDto> Handle(GetDistributedDonationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _distributedDonationService.GetDistributedDonationByIdAsync(request.Id);
        }
    }
}
