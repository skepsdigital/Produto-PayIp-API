using PayIP.Infra.Interfaces;
using PayIP.Model;
using PayIP.Services.Interfaces;

namespace PayIP.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> logger;
        private readonly IBlipSenderNotification _blipSender;

        public NotificationService(ILogger<NotificationService> logger, IBlipSenderNotification blipSenderNotification)
        {
            this.logger = logger;
            _blipSender = blipSenderNotification;
        }

        public async Task<object> SendNotification(NotificationRequest NotificationRequest)
        {
            if(!NotificationRequest.Phone.StartsWith("55"))
            {
                NotificationRequest.Phone = "55" + NotificationRequest.Phone;
            }
            var campaingRequest = new CampaignRequest()
            {
                Id = Guid.NewGuid().ToString(),
                Method = "SET",
                To = "postmaster@activecampaign.msging.net",
                Uri = "/campaign/full",
                Type = "application/vnd.iris.activecampaign.full-campaign+json",
                Resource = new Resource()
                {
                    Campaign = new Campaign()
                    {
                        Name = Guid.NewGuid().ToString(),
                        CampaignType = "Batch",
                        FlowId = NotificationRequest.FlowId,
                        StateId = NotificationRequest.StateId,
                        MasterState = NotificationRequest.BotSlug + "@msging.net",
                        ChannelType = "WhatsApp"
                    },
                    Message = new Message()
                    {
                        MessageTemplate = NotificationRequest.TemplateName,
                        MessageParams = new List<string>() { "1","2","3","4","5","6","7"},
                        ChannelType = "WhatsApp"
                    },
                    Audiences = new List<Audience>()
                    {
                        new Audience()
                        {
                            Recipient = "+" + NotificationRequest.Phone,
                            MessageParams =  NotificationRequest.Parameters
                                                        .Select((parameter, index) => new { Key = index + 1, Value = parameter.Text })
                                                        .ToDictionary(item => item.Key.ToString(), item => item.Value)
                        }
                    }
                }
            };

            campaingRequest.Resource.Message.MessageParams = campaingRequest.Resource.Audiences.First().MessageParams.Select(d => d.Key).ToList();

            if (!string.IsNullOrEmpty(NotificationRequest.PixCode))
            {
                campaingRequest.Resource.Audiences.First().MessageParams.Add("PixCode", NotificationRequest.PixCode);

            }

            if (!string.IsNullOrEmpty(NotificationRequest.TaxPayerId))
            {
                campaingRequest.Resource.Audiences.First().MessageParams.Add("taxpayerid", NotificationRequest.TaxPayerId);

            }
            var responseStatus = await _blipSender.SendNotificationAsync(campaingRequest, NotificationRequest.RouterBotKey);

            return responseStatus;
        }

        public async Task<object[]> SendNotifications(List<NotificationRequest> NotificationsRequest)
        {
            var tasks = NotificationsRequest.Select(async notificationRequest =>
            {
                return await SendNotification(notificationRequest);
            });

            var results = await Task.WhenAll(tasks);
            return results;
        }
    }
}
