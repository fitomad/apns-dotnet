namespace Apns.Validation;

internal sealed class NullableProperty<TValue> : IProperty<TValue> where TValue : struct, IEquatable<TValue>, IComparable<TValue>
{
    private readonly TValue? _value;
    private readonly IRule _rule;

    internal NullableProperty(TValue? value, IRule rule)
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
        if(_value is not null)
        {
            _rule.Check(() => lowerBound.CompareTo(_value.Value) < 0);
            _rule.Check(() => upperBound.CompareTo(_value.Value) > 0);
        }
        else
        {
            _rule.Check(() => false);
        }

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
