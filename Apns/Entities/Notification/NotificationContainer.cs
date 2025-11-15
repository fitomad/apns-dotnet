using System.Text.Json.Serialization;

namespace Fitomad.Apns.Entities.Notification;

public class NotificationContainer
{
    [JsonPropertyName("aps")]
    public Notification? Notification { get; init; }
}