using System.Text.Json;

namespace Apns.Entities.Notification;

public record AlertSettings
{   
    public string Title { get; private set; }
    public string Subtitle { get; private set; }
    public string Body { get; private set; }
    public string LaunchImage { get; private set; }
    public string TitleLocalizationKey { get; private set; }
    public string[] TitleLocalizationArguments { get; private set; }
    public string SubtitleLocalizationKey { get; private set; }
    public string[] SubtitleLocalizationArguments { get; private set; }
    public string BodyLocalizationKey { get; private set; }
    public string[] BodyLocalizationArguments { get; private set; }
}