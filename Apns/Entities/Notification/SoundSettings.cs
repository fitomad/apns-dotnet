using System.Text.Json.Serialization;

namespace Fitomad.Apns.Entities.Notification;

public record SoundSettings
{
    [JsonPropertyName("critical")]
    public int Critial { get; init; }
    [JsonPropertyName("name")] 
    public string Name { get; init; } = "default";
    [JsonPropertyName("volume")] 
    public double Volume { get; init; } = 0.50;
}
