using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.ThankYouNotes
{
    public class GetThankYouNoteByIdQuery : IRequest<ThankYouNoteDto>
    {
        public int Id { get; set; }
    }
}
