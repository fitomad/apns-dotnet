using System.Text.Json;

namespace Apns.Entities.Notification;

public record SoundSettings
{
    public int Critial { get; private set; }
    public string Name { get; private set; }
    public double Volume { get; private set; }
}