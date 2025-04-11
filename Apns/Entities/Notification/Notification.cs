using System.Text.Json.Serialization;

namespace Apns.Entities.Notification;

public struct NotificationContainer
{
    [JsonPropertyName("aps")]
    public Notification Notification { get; set; }
}
public record Notification
{
    [JsonPropertyName("alert")]
    public AlertBase Alert { get; internal set; }
    [JsonPropertyName("badge")]
    public int? Badge { get; internal set; }
    [JsonPropertyName("sound")]
    public string SoundName { get; internal set; }
    [JsonPropertyName("sound")]
    public SoundSettings Sound { get; internal set; }
    [JsonPropertyName("thread-id")]
    public string ThreadId { get; internal set; }
    [JsonPropertyName("category")]
    public string Category { get; internal set; }
    [JsonPropertyName("content-available")]
    public int? ContentAvailable { get; internal set; }
    [JsonPropertyName("mutable-content")]
    public int? MutableContent  { get; internal set; }
    [JsonPropertyName("target-content-id")]
    public string TargetContentId { get; internal set; }
    [JsonPropertyName("interruption-level")]
    public string InterruptionLevel { get; internal set; }
    [JsonPropertyName("relevance-score")]
    public double? RelevanceScore { get; internal set; }
    [JsonPropertyName("filter-criteria")]
    public string FilterCriteria { get; internal set; } 
    [JsonPropertyName("stale-date")]
    public int? StaleDate { get; internal set; }
    [JsonPropertyName("content-state")]
    public IDictionary<string, object> ContentState { get; internal set; }
    [JsonPropertyName("timestamp")]
    public int? Timestamp { get; internal set; }
    [JsonPropertyName("event")]
    public string Event { get; internal set; }
    [JsonPropertyName("dismissal-date")]
    public int? DismissalDate { get; internal set; }
    [JsonPropertyName("attributes-type")]
    public string AttributesType { get; internal set; }
    [JsonPropertyName("attributes")]
    public IDictionary<string, object> Attributes { get; internal set; }
}