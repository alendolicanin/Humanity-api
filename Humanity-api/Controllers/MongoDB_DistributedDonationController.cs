using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Humanity.Application.Services_MongoDB;

namespace Humanity_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoDB_DistributedDonationController : ControllerBase
    {
        private readonly DistributedDonationService _distributedDonationService;

        public MongoDB_DistributedDonationController(DistributedDonationService distributedDonationService)
        {
            _distributedDonationService = distributedDonationService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetDistributedDonationById(string id)
        {
            try
            {
                var result = await _distributedDonationService.GetDistributedDonationByIdAsync(id);
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
        public async Task<IActionResult> GetAllDistributedDonations()
        {
            try
            {
                var result = await _distributedDonationService.GetAllDistributedDonationsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDistributedDonation([FromBody] MongoDB_CreateDistributedDonationDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdDonation = await _distributedDonationService.CreateDistributedDonationAsync(createDto);
                return CreatedAtAction(nameof(GetDistributedDonationById), new { id = createdDonation.Id }, createdDonation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDistributedDonation(string id)
        {
            try
            {
                var success = await _distributedDonationService.DeleteDistributedDonationAsync(id);
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
