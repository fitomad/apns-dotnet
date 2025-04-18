using System.Runtime.CompilerServices;

namespace Fitomad.Apns.Entities.Notification;

public sealed class Volume : ApnsEnumeration, IApnsRepresentable, IEquatable<Volume>, IComparable<Volume>
{
    public static readonly Volume Silent = new(1, "silent", 0.0);
    public static readonly Volume Low = new(2, "low", 0.25);
    public static readonly Volume Medium = new(3, "medium", 0.50);
    public static readonly Volume High = new(4, "high", 0.75);
    public static readonly Volume Full = new(5, "full", 1.0);

    public double Decibels { get; init; }

    public Volume(int key, string value, double decibels) : base(key, value)
    {
        Decibels = decibels;
    }
    
    public string GetApnsString() => Value;
    
    public bool Equals(Volume? other)
    {
        if(other is not null)
        {
            return Key == other.Key && Decibels == other.Decibels;
        }
        
        return false;
    }

    public int CompareTo(Volume? other)
    {
        return Key.CompareTo(other.Key);
    }
}
