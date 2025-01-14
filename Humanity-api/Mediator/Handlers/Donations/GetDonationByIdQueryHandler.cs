using Humanity.API.Mediator.Queries.Donations;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Donations
{
    public class GetDonationByIdQueryHandler : IRequestHandler<GetDonationByIdQuery, DonationDto>
    {
        private readonly IDonationService _donationService;

        public GetDonationByIdQueryHandler(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public async Task<DonationDto> Handle(GetDonationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _donationService.GetDonationByIdAsync(request.Id);
        }
    }
}
