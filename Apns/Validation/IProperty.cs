namespace Apns.Validation;

internal interface IProperty<TValue> where TValue : IEquatable<TValue>, IComparable<TValue>
{
    IRule IsEqualsTo(TValue value);
    IRule InRange(TValue lowerBound, TValue upperBound);
    IRule IsNull();
    IRule IsNotNull();
}