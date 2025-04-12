namespace Apns.Entities;

internal interface IApnsRepresentable
{
    string GetApnsString();
}

public abstract class ApnsEnumeration
{
    public string Value { get; private set; }
    public int Key { get; private set; }

    protected internal ApnsEnumeration(int key, string value)
    {
        Key = key;
        Value = value;
    }

    public override bool Equals(object obj)
    {
        if(obj is not ApnsEnumeration otherEnumeration)
        {
            return false;
        }
        
        var typeMatches = GetType().Equals(obj.GetType());
        var keyMatches = Key.Equals(otherEnumeration.Key);

        return typeMatches && keyMatches;
    }
}
