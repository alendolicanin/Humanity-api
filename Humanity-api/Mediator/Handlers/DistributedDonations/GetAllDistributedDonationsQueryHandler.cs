using Humanity.API.Mediator.Queries.DistributedDonations;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.DistributedDonations
{
    public class GetAllDistributedDonationsQueryHandler : IRequestHandler<GetAllDistributedDonationsQuery, IEnumerable<DistributedDonationDto>>
    {
        private readonly IDistributedDonationService _distributedDonationService;

        public GetAllDistributedDonationsQueryHandler(IDistributedDonationService distributedDonationService)
        {
            _distributedDonationService = distributedDonationService;
        }

        public async Task<IEnumerable<DistributedDonationDto>> Handle(GetAllDistributedDonationsQuery request, CancellationToken cancellationToken)
        {
            return await _distributedDonationService.GetAllDistributedDonationsAsync();
        }
    }
}
