using Castle.DynamicProxy;
using ObjectChangeTracking.Abstractions;
using Core.Reflection;
using System.Reflection;

namespace ObjectChangeTracking.Interceptors;

class ObjectInterceptor : BaseInterceptor
{
    public ObjectInterceptor(ObjectTrackingState objectTrackingState)
        : base(objectTrackingState)
    {
    }

    public override void Intercept(IInvocation invocation)
    {
        //get is changed?
        if (invocation.Method.IsGetterMethod(nameof(ITrackableObject.IsChanged)))
        {
            invocation.ReturnValue = ObjectTrackingState.AnyChanges();
            return;
        }

        //get changed properties
        if (invocation.Method.IsGetterMethod(nameof(ITrackableObject.ChangedProperties)))
        {
            invocation.ReturnValue = ObjectTrackingState.GetChangedProperties();
            return;
        }

        //getter?
        if (invocation.Method.IsGetterMethod())
        {
            string property = invocation.Method.GetPropertyName();

            PropertyInfo? p = invocation.InvocationTarget.GetType().GetProperty(property);
            bool collection = p.PropertyType.IsCollection();

            if (collection)
            {
                ITrackableCollection? col = ObjectTrackingState.GetCollection(property);

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
        //setter?
        else if (invocation.Method.IsSetterMethod())
        {
            string propertyName = invocation.Method.GetPropertyName();

            PropertyAccessor accessor = PropertyAccessor.Get(invocation.TargetType.GetProperty(propertyName));

            ObjectTrackingState.AddChangedProperty(propertyName, accessor.GetValue(invocation.InvocationTarget));

            invocation.Proceed();
        }
        else
        {
            invocation.Proceed();

            //stop leaking "this"
            if (invocation.ReturnValue == invocation.InvocationTarget)
            {
                invocation.ReturnValue = invocation.Proxy;
            }
        }
    }
}
