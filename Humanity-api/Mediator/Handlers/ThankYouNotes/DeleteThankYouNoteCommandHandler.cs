using Humanity.API.Mediator.Commands.ThankYouNotes;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.ThankYouNotes
{
    public class DeleteThankYouNoteCommandHandler : IRequestHandler<DeleteThankYouNoteCommand, bool>
    {
        private readonly IThankYouNoteService _thankYouNoteService;

        public DeleteThankYouNoteCommandHandler(IThankYouNoteService thankYouNoteService)
        {
            _thankYouNoteService = thankYouNoteService;
        }

        public async Task<bool> Handle(DeleteThankYouNoteCommand request, CancellationToken cancellationToken)
        {
            return await _thankYouNoteService.DeleteThankYouNoteAsync(request.Id);
        }
    }
}
