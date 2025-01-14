using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Commands.ThankYouNotes
{
    public class CreateThankYouNoteCommand : IRequest<ThankYouNoteDto>
    {
        public CreateThankYouNoteDto CreateThankYouNoteDto { get; set; }
    }
}
