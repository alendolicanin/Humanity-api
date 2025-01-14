using MediatR;

namespace Humanity.API.Mediator.Commands.ThankYouNotes
{
    public class DeleteThankYouNoteCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
