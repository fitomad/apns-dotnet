namespace Fitomad.Apns.Entities.Notification;

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