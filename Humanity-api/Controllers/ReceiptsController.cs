using Humanity.API.Mediator.Commands.Receipts;
using Humanity.API.Mediator.Queries.Receipts;
using Humanity.Application.DTOs;
using Humanity_api.Mediator.Commands.Receipts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Humanity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Akcija za dobijanje računa po ID-u (Admin može videti račune)
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetReceiptById(int id)
        {
            try
            {
                var receipt = await _mediator.Send(new GetReceiptByIdQuery { Id = id });
                return Ok(receipt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje svih računa (Admin može videti sve račune)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReceipts()
        {
            try
            {
                var receipts = await _mediator.Send(new GetAllReceiptsQuery());
                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirm-signature")]
        public async Task<IActionResult> ConfirmSignature(int receiptId, string recipientId)
        {
            try
            {
                var result = await _mediator.Send(new ConfirmSignatureCommand
                {
                    ReceiptId = receiptId,
                    RecipientId = recipientId
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za brisanje računa (Admin može brisati račune)
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteReceiptCommand { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
