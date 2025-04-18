namespace Fitomad.Apns.Services.Validation;

internal interface IRule
{
    bool Validate();

    IRule Where(Func<bool> condition);
    IRule VerifyThat(Func<bool> condition);
    IRule OnFailure(Action action);
    IRule OnSuccess(Action action);
    IProperty<TValue> Property<TValue>(TValue value) where TValue : IEquatable<TValue>, IComparable<TValue>;
    IProperty<TValue> Property<TValue>(TValue? value) where TValue : struct, IEquatable<TValue>, IComparable<TValue>;
}
