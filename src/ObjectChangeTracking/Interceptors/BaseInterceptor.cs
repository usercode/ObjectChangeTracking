using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking.Interceptors
{
    abstract class BaseInterceptor : IInterceptor
    {
        public BaseInterceptor(ObjectTrackingState objectTrackingState)
        {
            ObjectTrackingState = objectTrackingState;
        }

        protected ObjectTrackingState ObjectTrackingState { get; }

        public abstract void Intercept(IInvocation invocation);
    }
}
