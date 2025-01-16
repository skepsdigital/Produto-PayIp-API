using PayIP.Model;

namespace PayIP.Infra.Interfaces
{
    public interface IBlipSenderNotification
    {
        Task<bool> SendNotificationAsync(CampaignRequest request, string routerKey);
    }
}
