namespace Apns.Validation;

internal interface IRule
{
    bool Validate();

    IRule When(Func<bool> condition);
    IRule Check(Func<bool> condition);
    IRule OnFailure(Action action);
    IRule OnSuccess(Action action);
    IProperty<TValue> Property<TValue>(TValue value) where TValue : IEquatable<TValue>, IComparable<TValue>;
    IProperty<TValue> Property<TValue>(TValue? value) where TValue : struct, IEquatable<TValue>, IComparable<TValue>;
}
