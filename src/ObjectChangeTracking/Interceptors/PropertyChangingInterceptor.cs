using Castle.DynamicProxy;
using ObjectChangeTracking.Interceptors;
using System.ComponentModel;

namespace ObjectChangeTracking;

class PropertyChangingInterceptor : BaseInterceptor
{
    private event PropertyChangingEventHandler? PropertyChanging;

    public PropertyChangingInterceptor(ObjectTrackingState objectTrackingState)
        : base(objectTrackingState)
    {
    }

    public override void Intercept(IInvocation invocation)
    {
        if (invocation.Method.IsSetterMethod())
        {
            RaisePropertyChanging(invocation.Proxy, invocation.Method.GetPropertyName());

            invocation.Proceed();
        }
        else
        {
            switch (invocation.Method.Name)
            {
                case $"{ReflectionHelper.AddPrefix}{nameof(INotifyPropertyChanging.PropertyChanging)}":
                    PropertyChanging += (PropertyChangingEventHandler)invocation.Arguments[0];
                    break;
                case $"{ReflectionHelper.RemovePrefix}{nameof(INotifyPropertyChanging.PropertyChanging)}":
                    PropertyChanging -= (PropertyChangingEventHandler)invocation.Arguments[0];
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }
    }

    private void RaisePropertyChanging(object proxy, string propertyName)
    {
        PropertyChanging?.Invoke(proxy, new PropertyChangingEventArgs(propertyName));
    }
}
