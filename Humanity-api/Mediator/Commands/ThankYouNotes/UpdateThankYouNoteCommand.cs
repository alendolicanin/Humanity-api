using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Commands.ThankYouNotes
{
    public class UpdateThankYouNoteCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public UpdateThankYouNoteDto UpdateThankYouNoteDto { get; set; }
    }
}
