using System.Text.Json.Serialization;

namespace Apns.Entities.Notification;

[JsonDerivedType(typeof(Alert))]
[JsonDerivedType(typeof(LocalizableAlert))]
public abstract class AlertBase
{
    [JsonPropertyName("launch-image")]
    public string LaunchImage { get; set; }
}

public sealed class Alert: AlertBase
{   
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; }
    [JsonPropertyName("body")]
    public string Body { get; set; }
    
}

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
/*
public sealed class AlertBuilder
{
    private Alert _settings;

    public AlertBuilder()
    {
        _settings = new Alert();
    }

    public AlertBuilder WithTitle(string title)
    {
        _settings.Title = title;
        return this;
    }

    public AlertBuilder WithSubtitle(string subtitle)
    {
        _settings.Subtitle = subtitle;
        return this;
    }

    public AlertBuilder WithBody(string body)
    {
        _settings.Body = body;
        return this;
    }

    public AlertBuilder WithLaunchImage(string launchImage)
    {
        _settings.LaunchImage = launchImage;
        return this;
    }

    public Alert Build()
    {
        return _settings;
    }

    public AlertBuilder Reset()
    {
        _settings = new Alert();
        return this;
    }
}

public sealed class LocalizedAlertBuilder
{
    private LocalizableAlert _settings;

    public LocalizedAlertBuilder()
    {
        _settings = new LocalizableAlert();
    }

    public LocalizedAlertBuilder WithTitleKey(string titleKey)
    {
        _settings.TitleLocalizationKey = titleKey;
        return this;
    }

    public LocalizedAlertBuilder WithTitleArgs(params string[] titleArgs)
    {
        _settings.TitleLocalizationArguments = titleArgs;
        return this;
    }

    public LocalizedAlertBuilder WithSubtitleKey(string subtitleKey)
    {
        _settings.SubtitleLocalizationKey = subtitleKey;
        return this;
    }

    public LocalizedAlertBuilder WithSubtitleArgs(params string[] subtitleArgs)
    {
        _settings.SubtitleLocalizationArguments = subtitleArgs;
        return this;
    }

    public LocalizedAlertBuilder WithBodyKey(string bodyKey)
    {
        _settings.BodyLocalizationKey = bodyKey;
        return this;
    }

    public LocalizedAlertBuilder WithBodyArgs(params string[] bodyArgs)
    {
        _settings.BodyLocalizationArguments = bodyArgs;
        return this;
    }

    public LocalizedAlertBuilder WithLaunchImage(string launchImage)
    {
        _settings.LaunchImage = launchImage;
        return this;
    }

    public LocalizableAlert Build()
    {
        return _settings;
    }

    public LocalizedAlertBuilder Reset()
    {
        _settings = new LocalizableAlert();
        return this;
    }
}
*/