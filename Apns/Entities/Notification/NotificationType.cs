namespace Fitomad.Apns.Entities.Notification;

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
