using Humanity.API.Mediator.Commands.ThankYouNotes;
using Humanity.API.Mediator.Queries.ThankYouNotes;
using Humanity.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Humanity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThankYouNoteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ThankYouNoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Akcija za kreiranje zahvalnice (Recipient može kreirati zahvalnice)
        [HttpPost("create")]
        public async Task<IActionResult> CreateThankYouNote(CreateThankYouNoteDto createThankYouNoteDto)
        {
            try
            {
                var thankYouNote = await _mediator.Send(new CreateThankYouNoteCommand { CreateThankYouNoteDto = createThankYouNoteDto });
                return Ok(thankYouNote);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje zahvalnice po ID-u (Admin i Donor mogu videti zahvalnice)
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetThankYouNoteById(int id)
        {
            try
            {
                var thankYouNote = await _mediator.Send(new GetThankYouNoteByIdQuery { Id = id });
                return Ok(thankYouNote);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje svih zahvalnica (Admin može videti sve zahvalnice)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllThankYouNotes()
        {
            try
            {
                var thankYouNotes = await _mediator.Send(new GetAllThankYouNotesQuery());
                return Ok(thankYouNotes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za ažuriranje zahvalnice (Admin može ažurirati zahvalnice)
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateThankYouNote(int id, UpdateThankYouNoteDto updateThankYouNoteDto)
        {
            try
            {
                var result = await _mediator.Send(new UpdateThankYouNoteCommand { Id = id, UpdateThankYouNoteDto = updateThankYouNoteDto });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za brisanje zahvalnice (Admin može brisati zahvalnice)
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteThankYouNote(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteThankYouNoteCommand { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
