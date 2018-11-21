using Castle.DynamicProxy;
using ObjectChangeTracking.Interceptors;
using System;
using System.ComponentModel;

namespace ObjectChangeTracking
{
    class PropertyChangedInterceptor : BaseInterceptor
    {
        private event PropertyChangedEventHandler PropertyChanged;

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
                    case "add_PropertyChanged":
                        PropertyChanged += (PropertyChangedEventHandler)invocation.Arguments[0];
                        break;
                    case "remove_PropertyChanged":
                        PropertyChanged -= (PropertyChangedEventHandler)invocation.Arguments[0];
                        break;
                    default:
                        invocation.Proceed();
                        break;
                }
            }
        }

        private void RaisePropertyChanged(Object proxy, String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(proxy, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
