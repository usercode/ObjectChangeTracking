using System.Reflection;

namespace Core.Reflection;

internal sealed class PropertyAccessor<TTarget, TProperty> : PropertyAccessor
{
    public PropertyAccessor(PropertyInfo propertyInfo)
        : base(propertyInfo)
    {
        if (propertyInfo.CanRead)
        {
            _getter = (Func<TTarget, TProperty>)
                                propertyInfo.GetGetMethod()!.CreateDelegate(typeof(Func<TTarget, TProperty>));
        }

        if (propertyInfo.CanWrite)
        {
            _setter = (Action<TTarget, TProperty>)
                                propertyInfo.GetSetMethod()!.CreateDelegate(typeof(Action<TTarget, TProperty>));
        }
    }

    private Func<TTarget, TProperty> _getter;
    private Action<TTarget, TProperty> _setter;

    public override object? GetValue(object target)
    {
        return _getter((TTarget)target);
    }

    public override void SetValue(object target, object? value)
    {
        _setter((TTarget)target, (TProperty)value);
    }
}
