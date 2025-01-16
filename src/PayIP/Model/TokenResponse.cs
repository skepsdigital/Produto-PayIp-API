using System.Text.Json.Serialization;

namespace PayIP.Model
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
