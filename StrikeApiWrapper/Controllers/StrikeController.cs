using Microsoft.AspNetCore.Mvc;
using StrikeApiWrapper.Services;

namespace StrikeApiWrapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StrikeController(IStrikeApiService strikeApiService) : ControllerBase
    {
        [HttpPost("invoices")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            var response = await strikeApiService.CreateInvoiceAsync(request.CorrelationId, request.Description, request.Currency, request.Amount);
            return Ok(response);
        }

        [HttpGet("account/{handle}/profile")]
        public async Task<IActionResult> GetProfile(string handle)
        {
            var response = await strikeApiService.GetAccountProfileAsync(handle);
            return Ok(response);
        }

        [HttpGet("invoices/{invoiceId}")]
        public async Task<IActionResult> GetInvoice(string invoiceId)
        {
            var response = await strikeApiService.GetInvoiceByIdAsync(invoiceId);
            return Ok(response);
        }

        [HttpPost("invoices/{invoiceId}/quote")]
        public async Task<IActionResult> GenerateQuote(string invoiceId)
        {
            var response = await strikeApiService.GenerateQuoteForInvoiceAsync(invoiceId);
            return Ok(response);
        }

        [HttpGet("invoices")]
        public async Task<IActionResult> GetUnpaidInvoices([FromQuery] int skip = 0, [FromQuery] int top = 10)
        {
            var response = await strikeApiService.GetUnpaidInvoicesAsync(skip, top);
            return Ok(response);
        }
    }

    public class CreateInvoiceRequest
    {
        public string CorrelationId { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
    }
}