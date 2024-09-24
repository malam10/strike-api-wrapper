using System.Text;
using Newtonsoft.Json;

namespace StrikeApiWrapper.Services
{
    public interface IStrikeApiService
    {
        Task<string> CreateInvoiceAsync(string correlationId, string description, string currency, string amount);
        Task<string> GetAccountProfileAsync(string handle);
        Task<string> GetInvoiceByIdAsync(string invoiceId);
        Task<string> GenerateQuoteForInvoiceAsync(string invoiceId);
        Task<string> GetUnpaidInvoicesAsync(int skip = 0, int top = 10);
    }

    public class StrikeApiService(HttpClient httpClient, string? apiKey, string? environment) : IStrikeApiService
    {
        private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, object content = null)
        {
            var request = new HttpRequestMessage(method, $"{environment}/{endpoint}");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Headers.Add("accept", "application/json");

            if (content != null)
            {
                var jsonContent = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            return request;
        }

        public async Task<string> CreateInvoiceAsync(string correlationId, string description, string currency, string amount)
        {
            var payload = new
            {
                correlationId,
                description,
                amount = new { currency, amount }
            };

            var request = CreateRequest(HttpMethod.Post, "v1/invoices", payload);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAccountProfileAsync(string handle)
        {
            var request = CreateRequest(HttpMethod.Get, $"v1/accounts/handle/{handle}/profile");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetInvoiceByIdAsync(string invoiceId)
        {
            var request = CreateRequest(HttpMethod.Get, $"v1/invoices/{invoiceId}");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GenerateQuoteForInvoiceAsync(string invoiceId)
        {
            var request = CreateRequest(HttpMethod.Post, $"v1/invoices/{invoiceId}/quote");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUnpaidInvoicesAsync(int skip = 0, int top = 10)
        {
            var request = CreateRequest(HttpMethod.Get, $"v1/invoices?$filter=state eq 'UNPAID'&$orderby=created asc&$skip={skip}&$top={top}");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}