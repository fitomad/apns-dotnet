using System.Text.Json.Serialization;

namespace Fitomad.Apns.Entities.Notification;

/// <summary>
/// Alert properties for a localized notification
/// </summary>
public sealed class LocalizableAlert: AlertBase
{   
    [JsonPropertyName("title-loc-key")]
    public string TitleLocalizationKey { get; init; }
    [JsonPropertyName("title-loc-args")]
    public object[] TitleLocalizationArguments { get; init; }
    [JsonPropertyName("subtitle-loc-key")]
    public string SubtitleLocalizationKey { get; init; }
    [JsonPropertyName("subtitle-loc-args")]
    public object[] SubtitleLocalizationArguments { get; init; }
    [JsonPropertyName("loc-key")]
    public string BodyLocalizationKey { get; init; }
    [JsonPropertyName("loc-args")]
    public object[] BodyLocalizationArguments { get; init; }
}