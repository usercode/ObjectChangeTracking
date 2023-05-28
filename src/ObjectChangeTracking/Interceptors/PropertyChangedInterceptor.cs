using Castle.DynamicProxy;
using ObjectChangeTracking.Interceptors;
using System.ComponentModel;

namespace ObjectChangeTracking;

class PropertyChangedInterceptor : BaseInterceptor
{
    private event PropertyChangedEventHandler? PropertyChanged;

    public PropertyChangedInterceptor(ObjectTrackingState objectTrackingState)
        : base(objectTrackingState)
    {
    }

    public override void Intercept(IInvocation invocation)
    {
        if (invocation.Method.IsSetterMethod())
        {
            invocation.Proceed();

            RaisePropertyChanged(invocation.Proxy, invocation.Method.GetPropertyName());
        }
        else
        {
            switch (invocation.Method.Name)
            {
                case $"{ReflectionHelper.AddPrefix}{nameof(INotifyPropertyChanged.PropertyChanged)}":
                    PropertyChanged += (PropertyChangedEventHandler)invocation.Arguments[0];
                    break;
                case $"{ReflectionHelper.RemovePrefix}{nameof(INotifyPropertyChanged.PropertyChanged)}":
                    PropertyChanged -= (PropertyChangedEventHandler)invocation.Arguments[0];
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }
    }

    private void RaisePropertyChanged(object proxy, string propertyName)
    {
        PropertyChanged?.Invoke(proxy, new PropertyChangedEventArgs(propertyName));
    }
}
