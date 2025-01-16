using Newtonsoft.Json;

namespace PayIP.Model
{
    public class NotificationRequest
    {
        [JsonProperty("routerBotKey")]
        public string RouterBotKey { get; set; }

        [JsonProperty("botKey")]
        public string BotKey { get; set; }

        [JsonProperty("botSlug")]
        public string BotSlug { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("templateName")]
        public string TemplateName { get; set; }

        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; }

        [JsonProperty("flowId")]
        public string FlowId { get; set; }

        [JsonProperty("stateId")]
        public string StateId { get; set; }

        [JsonProperty("pixCode")]
        public string PixCode { get; set; }

        [JsonProperty("identificadorCliente")]
        public string? TaxPayerId { get; set; }
    }

    public class Parameter
    {
        [JsonProperty("componentType")]
        public string ComponentType { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
