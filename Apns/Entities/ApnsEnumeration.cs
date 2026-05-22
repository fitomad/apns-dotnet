namespace Fitomad.Apns.Entities;

internal interface IApnsRepresentable
{
    string GetApnsString();
}

public abstract class ApnsEnumeration : IEquatable<ApnsEnumeration?>
{
    public string Value { get; private set; }
    public int Key { get; private set; }

    protected internal ApnsEnumeration(int key, string value)
    {
        Key = key;
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if(obj is not ApnsEnumeration otherEnumeration)
        {
            return false;
        }
        
        var typeMatches = GetType().Equals(obj.GetType());
        var keyMatches = Key.Equals(otherEnumeration.Key);

        return typeMatches && keyMatches;
    }

    public bool Equals(ApnsEnumeration? other)
    {
        return other is not null &&
               Value == other.Value &&
               Key == other.Key;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Key);
    }

    public static bool operator ==(ApnsEnumeration? left, ApnsEnumeration? right)
    {
        return EqualityComparer<ApnsEnumeration>.Default.Equals(left, right);
    }

    public static bool operator !=(ApnsEnumeration? left, ApnsEnumeration? right)
    {
        return !(left == right);
    }
}
