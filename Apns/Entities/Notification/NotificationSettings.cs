namespace Apns.Entities.Notification;

public record NotificationSettings
{
    public string NotificationId { get; set; }
    public NotificationType PushType { get; set; }
    public int? ExpirationTime { get; set; }
    public Priority Priority { get; set; }
    public string CollapseId { get; set; }

    public static readonly NotificationSettings Default = new(){ Priority = Priority.High };
}