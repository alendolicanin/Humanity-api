using Humanity.API.Mediator.Queries.ThankYouNotes;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.ThankYouNotes
{
    public class GetThankYouNoteByIdQueryHandler : IRequestHandler<GetThankYouNoteByIdQuery, ThankYouNoteDto>
    {
        private readonly IThankYouNoteService _thankYouNoteService;

        public GetThankYouNoteByIdQueryHandler(IThankYouNoteService thankYouNoteService)
        {
            _thankYouNoteService = thankYouNoteService;
        }

        public async Task<ThankYouNoteDto> Handle(GetThankYouNoteByIdQuery request, CancellationToken cancellationToken)
        {
            return await _thankYouNoteService.GetThankYouNoteByIdAsync(request.Id);
        }
    }
}
