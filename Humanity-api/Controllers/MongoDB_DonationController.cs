using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Humanity.Application.Services_MongoDB;

namespace Humanity_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoDB_DonationController : ControllerBase
    {
        private readonly DonationService _donationService;

        public MongoDB_DonationController(DonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetDonationById(string id)
        {
            try
            {
                var result = await _donationService.GetDonationByIdAsync(id);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDonations()
        {
            try
            {
                var result = await _donationService.GetAllDonationsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDonation([FromBody] MongoDB_CreateDonationDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdDonation = await _donationService.CreateDonationAsync(createDto);
                return CreatedAtAction(nameof(GetDonationById), new { id = createdDonation.Id }, createdDonation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDonation(string id, [FromBody] MongoDB_UpdateDonationDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _donationService.UpdateDonationAsync(id, updateDto);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDonation(string id)
        {
            try
            {
                var success = await _donationService.DeleteDonationAsync(id);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
