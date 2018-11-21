using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ObjectChangeTracking.Abstractions;

namespace ObjectChangeTracking.Interceptors
{
    class ObjectInterceptor : BaseInterceptor
    {
        public ObjectInterceptor(ObjectTrackingState objectTrackingState)
            : base(objectTrackingState)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            if (invocation.Method.IsGetterMethod(nameof(ITrackableObject.IsChanged)))
            {
                invocation.ReturnValue = ObjectTrackingState.AnyChanges();
                return;
            }

            if (invocation.Method.IsGetterMethod(nameof(ITrackableObject.ChangedProperties)))
            {
                invocation.ReturnValue = ObjectTrackingState.GetChangedProperties();
                return;
            }

            if (invocation.Method.IsGetterMethod())
            {
                String property = invocation.Method.GetPropertyName();

                var p = invocation.InvocationTarget.GetType().GetProperty(property);
                bool collection = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>);

                if (collection)
                {
                    Object col = ObjectTrackingState.GetCollection(property);

                    if (col == null)
                    {
                        invocation.Proceed();

                        col = invocation.ReturnValue.AsTrackingCollection(property, ObjectTrackingState);

                        ObjectTrackingState.SetCollection(property, col);
                    }

                    invocation.ReturnValue = col;
                }
                else
                {
                    invocation.Proceed();
                }
            }
            else if (invocation.Method.IsSetterMethod())
            {
                String propertyName = invocation.Method.GetPropertyName();

                ObjectTrackingState.AddChangedProperty(propertyName, null);

                invocation.Proceed();
            }            
        }
    }
}
