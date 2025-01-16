using Humanity.API.Mediator.Commands.DistributedDonations;
using Humanity.API.Mediator.Queries.DistributedDonations;
using Humanity.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Humanity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributedDonationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DistributedDonationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Akcija za kreiranje podeljene donacije
        [HttpPost("create")]
        public async Task<IActionResult> CreateDistributedDonation(CreateDistributedDonationDto createDistributedDonationDto)
        {
            try
            {
                var distributedDonation = await _mediator.Send(new CreateDistributedDonationCommand { CreateDistributedDonationDto = createDistributedDonationDto });
                return Ok(distributedDonation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje podeljene donacije po ID-u
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetDistributedDonationById(int id)
        {
            try
            {
                var distributedDonation = await _mediator.Send(new GetDistributedDonationByIdQuery { Id = id });
                return Ok(distributedDonation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje svih podeljenih donacija
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDistributedDonations()
        {
            try
            {
                var distributedDonations = await _mediator.Send(new GetAllDistributedDonationsQuery());
                return Ok(distributedDonations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za brisanje podeljene donacije
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDistributedDonation(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteDistributedDonationCommand { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
