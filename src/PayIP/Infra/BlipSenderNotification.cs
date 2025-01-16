using PayIP.Infra.Interfaces;
using System.Text.Json;
using System.Text;
using PayIP.Model;

namespace PayIP.Infra
{
    public class BlipSenderNotification : IBlipSenderNotification
    {
        private readonly ILogger<BlipSenderNotification> logger;
        private readonly HttpClient _httpClient;

        private readonly string URL_BASE = "https://payip.http.msging.net/commands";

        public BlipSenderNotification(ILogger<BlipSenderNotification> logger, IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            _httpClient = clientFactory.CreateClient();
        }

        public async Task<bool> SendNotificationAsync(CampaignRequest request, string routerKey)
        {
            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, URL_BASE)
            {
                Content = content
            };
            httpRequest.Headers.Add("Authorization", "Key "+ routerKey);

            var response = await _httpClient.SendAsync(httpRequest);

            var respo = await response.Content.ReadAsStringAsync();

            logger.LogInformation(respo);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}
