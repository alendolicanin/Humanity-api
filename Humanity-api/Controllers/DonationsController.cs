using Humanity.API.Mediator.Commands.Donations;
using Humanity.API.Mediator.Queries.Donations;
using Humanity.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Humanity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DonationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Akcija za kreiranje donacije
        [HttpPost("create")]
        public async Task<IActionResult> CreateDonation(CreateDonationDto createDonationDto)
        {
            try
            {
                if (createDonationDto == null)
                {
                    return BadRequest("Invalid donation data.");
                }

                var donation = await _mediator.Send(new CreateDonationCommand { CreateDonationDto = createDonationDto });
                return Ok(donation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje donacije po ID-u
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetDonationById(int id)
        {
            try
            {
                var donation = await _mediator.Send(new GetDonationByIdQuery { Id = id });
                return Ok(donation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje svih donacija
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDonations([FromQuery] DonationQueryDto queryDto)
        {
            try
            {
                var donations = await _mediator.Send(new GetAllDonationsQuery { QueryDto = queryDto });
                return Ok(donations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za ažuriranje donacije
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDonation(int id, UpdateDonationDto updateDonationDto)
        {
            try
            {
                var result = await _mediator.Send(new UpdateDonationCommand { Id = id, UpdateDonationDto = updateDonationDto });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za brisanje donacije
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteDonationCommand { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
