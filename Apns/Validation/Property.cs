namespace Apns.Validation;

internal sealed class Property<TValue> : IProperty<TValue> where TValue : IEquatable<TValue>, IComparable<TValue>
{
    private readonly TValue _value;
    private readonly IRule _rule;

    internal Property(TValue value, IRule rule)
    {
        _value = value;
        _rule = rule;
    }

    public IRule IsEqualsTo(TValue value)
    {
        _rule.Check(() => _value.Equals(value));
        return _rule;
    }

    public IRule InRange(TValue lowerBound, TValue upperBound)
    {
        _rule.Check(() => lowerBound.CompareTo(_value) < 0);
        _rule.Check(() => upperBound.CompareTo(_value) > 0);
        
        return _rule;
    }

    public IRule IsNull()
    {
        _rule.Check(() => _value == null);
        return _rule;
    }

    public IRule IsNotNull()
    {
        _rule.Check(() => _value != null);
        return _rule;
    }
}
