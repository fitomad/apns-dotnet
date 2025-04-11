namespace Apns.Entities.Notification;

public enum NotificationType
{
    Alert,
    Background,
    Controls,
    Location,
    VoIP,
    Complication,
    FileProvider,
    Mdm,
    LiveActivity,
    PushToTalk
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