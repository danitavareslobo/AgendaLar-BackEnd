using AgendaLarAPI.Models.Notification;
using System.ComponentModel.DataAnnotations;

namespace AgendaLarAPI.Services
{
    public class NotificationService
    {
        public List<Notification> Notifications { get; private set; }

        public bool HasNotifications => Notifications.Any();

        public NotificationService()
        {
            Notifications = new List<Notification>();
        }

        public void AddNotification(string key, string message, NotificationType type = NotificationType.Information)
        {
            Notifications.Add(new Notification(key, message, type));
        }
        public void AddNotification(Notification notification)
        {
            Notifications.Add(notification);
        }

        public void ClearNotification()
        {
            Notifications.Clear();
        }

        public void AddNotifications(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddNotification(error.ErrorCode, error.ErrorMessage, NotificationType.Warning);
            }
        }
    }
}

