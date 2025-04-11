using System.Text.Json;
using System.Text.Json.Serialization;

namespace Apns.Entities.Notification;

public record SoundSettings
{
    [JsonPropertyName("critical")]
    public int Critial { get; init; }
    [JsonPropertyName("name")] 
    public string Name { get; init; } = "default";
    [JsonPropertyName("volume")] 
    public double Volume { get; init; } = 0.50;
}

public enum Volume
{
    Silent,
    Low,
    Medium,
    High,
    Full
}

public static class VolumeExtensions
{
    public static double GetApnsValue(this Volume volume)
    {
        var value = volume switch
        {
            Volume.Silent => 0,
            Volume.Low => 0.25,
            Volume.Medium => 0.50,
            Volume.High => 0.75,
            Volume.Full => 1.0,
            _ => 0.50
        };

        return value;
    }
}