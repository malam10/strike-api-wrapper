using Microsoft.AspNetCore.Mvc;
using Moq;
using StrikeApiWrapper.Controllers;
using StrikeApiWrapper.Services;

namespace StrikeApiWrapperTests
{
    public class StrikeControllerTests
    {
        private readonly Mock<IStrikeApiService> _mockStrikeApiService;
        private readonly StrikeController _controller;

        public StrikeControllerTests()
        {
            _mockStrikeApiService = new Mock<IStrikeApiService>();
            _controller = new StrikeController(_mockStrikeApiService.Object);
        }

        [Fact]
        public async Task CreateInvoice_ReturnsOk_WithInvoiceResponse()
        {
            var request = new CreateInvoiceRequest
            {
                CorrelationId = "123",
                Description = "Test invoice",
                Currency = "USD",
                Amount = "100"
            };

            var expectedResponse = "{\"invoiceId\":\"inv-123\"}";
            _mockStrikeApiService
                .Setup(service => service.CreateInvoiceAsync(request.CorrelationId, request.Description, request.Currency, request.Amount))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.CreateInvoice(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task GetProfile_ReturnsOk_WithProfileResponse()
        {
            var handle = "test-handle";
            var expectedResponse = "{\"profile\":\"Test Profile\"}";

            _mockStrikeApiService
                .Setup(service => service.GetAccountProfileAsync(handle))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GetProfile(handle) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task GetInvoice_ReturnsOk_WithInvoiceResponse()
        {
            var invoiceId = "inv-123";
            var expectedResponse = "{\"invoiceId\":\"inv-123\"}";

            _mockStrikeApiService
                .Setup(service => service.GetInvoiceByIdAsync(invoiceId))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GetInvoice(invoiceId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task GenerateQuote_ReturnsOk_WithQuoteResponse()
        {
            var invoiceId = "inv-123";
            var expectedResponse = "{\"quoteId\":\"quote-123\"}";

            _mockStrikeApiService
                .Setup(service => service.GenerateQuoteForInvoiceAsync(invoiceId))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GenerateQuote(invoiceId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task GetUnpaidInvoices_ReturnsOk_WithInvoicesResponse()
        {
            var expectedResponse = "{\"invoices\":[\"inv1\",\"inv2\"]}";
            _mockStrikeApiService
                .Setup(service => service.GetUnpaidInvoicesAsync(0, 10))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.GetUnpaidInvoices(0, 10) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }
    }
}