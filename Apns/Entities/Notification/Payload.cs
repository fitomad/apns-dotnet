using System.Text.Json;

namespace Apns.Entities.Notification;

public record Payload
{
    public AlertSettings Alert { get; private set; }
    public int Badge { get; private set; }
    public string SoundName { get; private set; }
    public SoundSettings Sound { get; private set; }
    public string ThreadId { get; private set; }
    public string Category { get; private set; }
    public int ContentAvailable { get; private set; }
    public int MutableContent  { get; private set; }
    public string TargetContentId { get; private set; }
    public string InterruptionLevel { get; private set; }
    public double RelevanceScore { get; private set; }
    public string FilterCriteria { get; private set; }
    public string StaleDate { get; private set; }
    public int Timestamp { get; private set; }
    public string Event { get; private set; }
    public int DismissalDate { get; private set; }
    public string AttributesType { get; private set; }
}