using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Humanity.Application.Services_Neo4jDB;

namespace Humanity_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Neo4jDB_ThankYouNotesController : ControllerBase
    {
        private readonly ThankYouNoteService _thankYouNoteService;

        public Neo4jDB_ThankYouNotesController(ThankYouNoteService thankYouNoteService)
        {
            _thankYouNoteService = thankYouNoteService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetThankYouNoteById(string id)
        {
            var result = await _thankYouNoteService.GetThankYouNoteByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllThankYouNotes()
        {
            var result = await _thankYouNoteService.GetAllThankYouNotesAsync();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateThankYouNote([FromBody] Neo4j_CreateThankYouNoteDto createDto)
        {
            // Proverava validaciju ulaznih podataka
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kreira novu zahvalnicu koristeći servis
            var createdNote = await _thankYouNoteService.CreateThankYouNoteAsync(createDto);
            // Vraća 201 Created s lokacijom i kreiranim objektom
            return CreatedAtAction(nameof(GetThankYouNoteById), new { id = createdNote.Id }, createdNote);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateThankYouNote(string id, [FromBody] Neo4j_UpdateThankYouNoteDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _thankYouNoteService.UpdateThankYouNoteAsync(id, updateDto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteThankYouNote(string id)
        {
            var success = await _thankYouNoteService.DeleteThankYouNoteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
