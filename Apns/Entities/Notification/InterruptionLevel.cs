namespace Fitomad.Apns.Entities.Notification;

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