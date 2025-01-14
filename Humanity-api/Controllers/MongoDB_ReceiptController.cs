using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Humanity.Application.Services_MongoDB;

namespace Humanity_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoDB_ReceiptController : ControllerBase
    {
        private readonly ReceiptService _receiptService;

        public MongoDB_ReceiptController(ReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetReceiptById(string id)
        {
            var result = await _receiptService.GetReceiptByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReceipts()
        {
            var result = await _receiptService.GetAllReceiptsAsync();
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReceipt(string id)
        {
            var success = await _receiptService.DeleteReceiptAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
