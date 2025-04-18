using Fitomad.Apns.Entities;

namespace Fitomad.Apns.Test.Mocks;

public class MockStatus: ApnsEnumeration, IApnsRepresentable, IEquatable<MockStatus>, IComparable<MockStatus>
{
    public static readonly MockStatus On = new(2, "on");
    public static readonly MockStatus StandBy = new(1, "stand-by");
    public static readonly MockStatus Off = new(0, "off");
    
    public MockStatus(int key, string value) : base(key, value)
    {
    }

    public bool Equals(MockStatus other)
    {
        return Key == other.Key && Value == other.Value;
    }

    public int CompareTo(MockStatus other)
    {
        return Key.CompareTo(other.Key);
    }

    public string GetApnsString()
    {
        return Value;
    }
}