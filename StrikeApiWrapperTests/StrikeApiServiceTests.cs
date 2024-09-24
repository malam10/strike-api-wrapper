using System.Net;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using StrikeApiWrapper.Services;

namespace StrikeApiWrapperTests
{
    public class StrikeApiServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private const string ApiKey = "27109D8C9046E28C86962E8A6A52E8BE350024CDB28A2B1CE496D893515E0EED";
        private const string Environment = "https://api.strike.com";
        private readonly StrikeApiService _service;

        public StrikeApiServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _service = new StrikeApiService(httpClient, ApiKey, Environment);
        }

        [Fact]
        public async Task CreateInvoiceAsync_ReturnsSuccessResponse()
        {
            var expectedResponse = JsonConvert.SerializeObject(new { invoiceId = "123" });
            SetupHttpMessageHandler(HttpMethod.Post, HttpStatusCode.OK, expectedResponse);

            var result = await _service.CreateInvoiceAsync("test-correlation-id", "Test Invoice", "USD", "100");

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetAccountProfileAsync_ReturnsSuccessResponse()
        {
            var expectedResponse = JsonConvert.SerializeObject(new { profile = "Test Profile" });
            SetupHttpMessageHandler(HttpMethod.Get, HttpStatusCode.OK, expectedResponse);

            var result = await _service.GetAccountProfileAsync("test-handle");

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetInvoiceByIdAsync_ReturnsSuccessResponse()
        {
            var expectedResponse = JsonConvert.SerializeObject(new { invoiceId = "123" });
            SetupHttpMessageHandler(HttpMethod.Get, HttpStatusCode.OK, expectedResponse);

            var result = await _service.GetInvoiceByIdAsync("123");

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GenerateQuoteForInvoiceAsync_ReturnsSuccessResponse()
        {
            var expectedResponse = JsonConvert.SerializeObject(new { quoteId = "quote-123" });
            SetupHttpMessageHandler(HttpMethod.Post, HttpStatusCode.OK, expectedResponse);

            var result = await _service.GenerateQuoteForInvoiceAsync("123");

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetUnpaidInvoicesAsync_ReturnsSuccessResponse()
        {
            var expectedResponse = JsonConvert.SerializeObject(new { invoices = new[] { "invoice1", "invoice2" } });
            SetupHttpMessageHandler(HttpMethod.Get, HttpStatusCode.OK, expectedResponse);

            var result = await _service.GetUnpaidInvoicesAsync();

            Assert.Equal(expectedResponse, result);
        }

        private void SetupHttpMessageHandler(HttpMethod method, HttpStatusCode statusCode, string responseContent)
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == method),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseContent),
                });
        }
    }
}