namespace Fitomad.Apns.Entities.Notification;

public sealed class Priority : ApnsEnumeration, IApnsRepresentable, IEquatable<Priority>, IComparable<Priority>
{
    public static readonly Priority High = new(10, "high");
    public static readonly Priority Medium = new(5, "medium");
    public static readonly Priority Low = new(1, "low");
    
    public Priority(int key, string value): base(key, value)
    {
    }
    
    public bool Equals(Priority other)
    {
        return Key == other.Key;
    }

    public int CompareTo(Priority other)
    {
        return Key.CompareTo(other.Key);
    }

    public string GetApnsString()
    {
        return $"{Key}";
    }
}
