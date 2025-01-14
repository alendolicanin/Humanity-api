using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.ThankYouNotes
{
    public class GetAllThankYouNotesQuery : IRequest<IEnumerable<ThankYouNoteDto>>
    {
    }
}
