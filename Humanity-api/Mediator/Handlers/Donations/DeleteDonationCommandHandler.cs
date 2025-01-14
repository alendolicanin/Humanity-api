using Humanity.API.Mediator.Commands.Donations;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Donations
{
    public class DeleteDonationHandler : IRequestHandler<DeleteDonationCommand, bool>
    {
        private readonly IDonationService _donationService;

        public DeleteDonationHandler(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public async Task<bool> Handle(DeleteDonationCommand request, CancellationToken cancellationToken)
        {
            return await _donationService.DeleteDonationAsync(request.Id);
        }
    }
}
