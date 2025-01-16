using PayIP.Model;

namespace PayIP.Services.Interfaces
{
    public interface INotificationService
    {
        Task<object> SendNotification(NotificationRequest NotificationRequest);
        Task<object[]> SendNotifications(List<NotificationRequest> NotificationsRequest);
    }
}
