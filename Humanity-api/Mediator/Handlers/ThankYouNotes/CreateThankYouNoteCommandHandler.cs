using Humanity.API.Mediator.Commands.ThankYouNotes;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.ThankYouNotes
{
    public class CreateThankYouNoteCommandHandler : IRequestHandler<CreateThankYouNoteCommand, ThankYouNoteDto>
    {
        private readonly IThankYouNoteService _thankYouNoteService;

        public CreateThankYouNoteCommandHandler(IThankYouNoteService thankYouNoteService)
        {
            _thankYouNoteService = thankYouNoteService;
        }

        public async Task<ThankYouNoteDto> Handle(CreateThankYouNoteCommand request, CancellationToken cancellationToken)
        {
            return await _thankYouNoteService.CreateThankYouNoteAsync(request.CreateThankYouNoteDto);
        }
    }
}
