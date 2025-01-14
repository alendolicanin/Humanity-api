using Humanity.API.Mediator.Queries.ThankYouNotes;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.ThankYouNotes
{
    public class GetAllThankYouNotesQueryHandler : IRequestHandler<GetAllThankYouNotesQuery, IEnumerable<ThankYouNoteDto>>
    {
        private readonly IThankYouNoteService _thankYouNoteService;

        public GetAllThankYouNotesQueryHandler(IThankYouNoteService thankYouNoteService)
        {
            _thankYouNoteService = thankYouNoteService;
        }

        public async Task<IEnumerable<ThankYouNoteDto>> Handle(GetAllThankYouNotesQuery request, CancellationToken cancellationToken)
        {
            return await _thankYouNoteService.GetAllThankYouNotesAsync();
        }
    }
}
