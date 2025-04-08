namespace Apns.Entities.Notification;

public record Notification
{
    public string NotificationId { set; private get; }
    public NotificationType PushType { set; private get; }
    public int ExpirationTime { set; private get; }
    public Priority NotificationPriority { set; private get; } = Priority.High;
    public string Topic { set; private get; }
    public string CollapseId { set; private get; }
    public Payload Payload { set; private get; }
}