namespace Apns.Entities.Notification;

public enum NotificationType
{
    Alert,
    Background,
    Controls,
    Location,
    VoIp,
    Complication,
    FileProvider,
    Mdm,
    LiveActivity,
    PushToTalk
}

public static class NotificationTypeExtensions
{
    public static string GetApnsValue(this NotificationType notificationType)
    {
        var value = notificationType switch
        {
            NotificationType.Alert => "alert",
            NotificationType.Background => "background",
            NotificationType.Controls => "controls",
            NotificationType.Location => "location",
            NotificationType.VoIp => "voip",
            NotificationType.Complication => "complication",
            NotificationType.FileProvider => "fileprovider",
            NotificationType.Mdm => "mdm",
            NotificationType.LiveActivity => "liveactivity",
            NotificationType.PushToTalk => "pushtotalk",
            _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
        };

        return value;
    }
}

public enum LiveActivityEvent
{
    Start,
    Update,
    End
}

public static class LiveActivityEventExtensions
{
    public static string GetApnsValue(this LiveActivityEvent liveActivityEvent)
    {
        var value = liveActivityEvent switch
        {
            LiveActivityEvent.Start => "start",
            LiveActivityEvent.Update => "update",
            LiveActivityEvent.End => "end",
            _ => throw new ArgumentOutOfRangeException(nameof(liveActivityEvent), liveActivityEvent, null)
        };

        return value;
    }
}

public enum InterruptionLevel
{
    Passive,
    Active,
    TimeSensitive,
    Critical
}

public static class InterruptionLevelExtensions
{
    public static string GetApnsValue(this InterruptionLevel interruptionLevel)
    {
        var value = interruptionLevel switch
        {
            InterruptionLevel.Passive => "passive",
            InterruptionLevel.Active => "active",
            InterruptionLevel.TimeSensitive => "time-sensitive",
            InterruptionLevel.Critical => "critical",
            _ => throw new ArgumentOutOfRangeException(nameof(interruptionLevel), interruptionLevel, null)
        };

        return value;
    }
}