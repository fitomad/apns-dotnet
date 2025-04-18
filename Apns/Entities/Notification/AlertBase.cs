using System.Text.Json.Serialization;

namespace Fitomad.Apns.Entities.Notification;

[JsonDerivedType(typeof(Alert))]
[JsonDerivedType(typeof(LocalizableAlert))]
public abstract class AlertBase
{
    [JsonPropertyName("launch-image")]
    public string LaunchImage { get; set; }
}