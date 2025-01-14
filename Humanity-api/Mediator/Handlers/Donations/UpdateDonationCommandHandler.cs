using Humanity.API.Mediator.Commands.Donations;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Donations
{
    public class UpdateDonationHandler : IRequestHandler<UpdateDonationCommand, bool>
    {
        private readonly IDonationService _donationService;

        public UpdateDonationHandler(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public async Task<bool> Handle(UpdateDonationCommand request, CancellationToken cancellationToken)
        {
            return await _donationService.UpdateDonationAsync(request.Id, request.UpdateDonationDto);
        }
    }
}
