using Humanity.API.Mediator.Queries.Donations;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Donations
{
    public class GetAllDonationsHandler : IRequestHandler<GetAllDonationsQuery, IEnumerable<DonationDto>>
    {
        private readonly IDonationService _donationService;

        public GetAllDonationsHandler(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public async Task<IEnumerable<DonationDto>> Handle(GetAllDonationsQuery request, CancellationToken cancellationToken)
        {
            return await _donationService.GetAllDonationsAsync(request.QueryDto);
        }
    }
}
