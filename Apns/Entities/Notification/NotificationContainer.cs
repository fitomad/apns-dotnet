using System.Text.Json.Serialization;

namespace Fitomad.Apns.Entities.Notification;

public struct NotificationContainer
{
    [JsonPropertyName("aps")]
    public Notification Notification { get; internal set; }
}