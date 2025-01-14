using Humanity.API.Mediator.Commands.Donations;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Donations
{
    public class CreateDonationHandler : IRequestHandler<CreateDonationCommand, DonationDto>
    {
        private readonly IDonationService _donationService;

        public CreateDonationHandler(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public async Task<DonationDto> Handle(CreateDonationCommand request, CancellationToken cancellationToken)
        {
            return await _donationService.CreateDonationAsync(request.CreateDonationDto);
        }
    }
}
