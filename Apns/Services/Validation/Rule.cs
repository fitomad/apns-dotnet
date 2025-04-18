namespace Fitomad.Apns.Services.Validation;

internal sealed class Rule: IRule
{
    private readonly List<Func<bool>> _conditions;
    private Func<bool>? _whenCondition;
    private Action? _failsAction;
    private Action? _successAction;

    internal Rule()
    {
        _conditions = new List<Func<bool>>();
    }
    
    public IRule VerifyThat(Func<bool> condition)
    {
        _conditions.Add(condition);
        return this;
    }

    public IRule Where(Func<bool> condition)
    {
        _whenCondition = condition;
        return this;
    }

    public IRule OnFailure(Action action)
    {
        _failsAction = action;
        return this;
    }

    public IRule OnSuccess(Action action)
    {
        _successAction = action;
        return this;
    }
    
    public IProperty<TValue> Property<TValue>(TValue value) where TValue : IEquatable<TValue>,IComparable<TValue>
    {
        var property = new Property<TValue>(value, this);
        return property;
    }
    
    public IProperty<TValue> Property<TValue>(TValue? value) where TValue : struct, IEquatable<TValue>, IComparable<TValue>
    {
        var property = new NullableProperty<TValue>(value, this);
        return property;
    }
    
    public bool Validate()
    {
        if(_whenCondition != null && !_whenCondition())
        {
            return true;
        }

        if(_conditions.Count > 0)
        {
            int failedConditionsCount = _conditions.Select(condition => condition())
                                                    .Where(result => result != true)
                                                    .Count();

            if(failedConditionsCount > 0)
            {
                if(_failsAction != null)
                {
                    _failsAction();
                }

                return false;
            }
        }

        if(_successAction != null)
        {
            _successAction();
        }
        
        return true;
    }
}
