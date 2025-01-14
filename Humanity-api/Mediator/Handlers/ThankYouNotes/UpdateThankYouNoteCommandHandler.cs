using Humanity.API.Mediator.Commands.ThankYouNotes;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.ThankYouNotes
{
    public class UpdateThankYouNoteCommandHandler : IRequestHandler<UpdateThankYouNoteCommand, bool>
    {
        private readonly IThankYouNoteService _thankYouNoteService;

        public UpdateThankYouNoteCommandHandler(IThankYouNoteService thankYouNoteService)
        {
            _thankYouNoteService = thankYouNoteService;
        }

        public async Task<bool> Handle(UpdateThankYouNoteCommand request, CancellationToken cancellationToken)
        {
            return await _thankYouNoteService.UpdateThankYouNoteAsync(request.Id, request.UpdateThankYouNoteDto);
        }
    }
}
