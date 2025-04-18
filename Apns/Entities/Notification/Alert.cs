using System.Text.Json.Serialization;

namespace Fitomad.Apns.Entities.Notification;

/// <summary>
/// Representation for the alert property in the notification payload
/// </summary>
public sealed class Alert: AlertBase
{   
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; }
    [JsonPropertyName("body")]
    public string Body { get; set; }
    
}
