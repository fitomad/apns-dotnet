namespace Fitomad.Apns.Services.Validation;

internal interface IProperty<TValue> where TValue : IEquatable<TValue>
{
    IRule IsEqualsTo(TValue value);
    IRule InRange(TValue lowerBound, TValue upperBound);
    IRule IsNull();
    IRule IsNotNull();
    IRule MatchRegularExpression(string expression);
}