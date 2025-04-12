namespace Apns.Entities.Notification;

public sealed class NotificationType : ApnsEnumeration, IApnsRepresentable, IEquatable<NotificationType>, IComparable<NotificationType>
{
    public static readonly NotificationType Alert = new(1, "alert");
    public static readonly NotificationType Background = new(2, "background");
    public static readonly NotificationType Controls = new(3, "controls");
    public static readonly NotificationType Location = new(4, "location");
    public static readonly NotificationType VoIp = new(5, "voip");
    public static readonly NotificationType Complication = new(6, "complication");
    public static readonly NotificationType FileProvider = new(7, "fileprovider");
    public static readonly NotificationType Mdm = new(8, "mdm");
    public static readonly NotificationType LiveActivity = new(9, "liveactivity");
    public static readonly NotificationType PushToTalk = new(10, "pushtotalk");
    
    public NotificationType(int key, string value) : base(key, value)
    {
    }

    public bool Equals(NotificationType other)
    {
        return Key == other.Key;
    }

    public int CompareTo(NotificationType other)
    {
        return Key.CompareTo(other.Key);
    }

    public string GetApnsString()
    {
        return Value;
    }
}


public sealed class LiveActivityEvent : ApnsEnumeration, IApnsRepresentable,IEquatable<LiveActivityEvent>, IComparable<LiveActivityEvent>
{
    public static readonly LiveActivityEvent Start = new(1, "start");
    public static readonly LiveActivityEvent Update = new(2, "update");
    public static readonly LiveActivityEvent End = new(3, "end");

    public LiveActivityEvent(int key, string value) : base(key, value)
    {
    }

    public bool Equals(LiveActivityEvent other)
    {
        return Key == other.Key;
    }

    public int CompareTo(LiveActivityEvent other)
    {
        return Key.CompareTo(other.Key);
    }

    public string GetApnsString()
    {
        return Value;
    }
}

public sealed class InterruptionLevel : ApnsEnumeration, IApnsRepresentable,IEquatable<InterruptionLevel>, IComparable<InterruptionLevel>
{
    public static readonly InterruptionLevel Passive = new(1, "passive");
    public static readonly InterruptionLevel Active = new(2, "active");
    public static readonly InterruptionLevel TimeSensitive = new(3, "time-sensitive");
    public static readonly InterruptionLevel Critical = new(4, "critical");

    public InterruptionLevel(int key, string value) : base(key, value)
    {
    }
    
    public bool Equals(InterruptionLevel other)
    {
        return Key == other.Key;
    }

    public int CompareTo(InterruptionLevel other)
    {
        return Key.CompareTo(other.Key);
    }

    public string GetApnsString()
    {
        return Value;
    }
}
