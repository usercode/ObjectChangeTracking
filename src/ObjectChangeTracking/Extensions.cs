using Castle.DynamicProxy;
using ObjectChangeTracking.Abstractions;
using ObjectChangeTracking.Interceptors;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObjectChangeTracking;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    internal static ProxyGenerator Generator = new ProxyGenerator();

    public static object AsTrackable(this object obj)
    {
        //is the object already a trackable object?
        if (obj is ITrackableObject)
        {
            return obj;
        }

        ObjectTrackingState objectTrackingState = new ObjectTrackingState(obj);

        object result = Generator.CreateClassProxyWithTarget(
                                        obj.GetType(),
                                        new[] { typeof(ITrackableObject), typeof(INotifyPropertyChanging), typeof(INotifyPropertyChanged) },
                                        obj,
                                        new PropertyChangingInterceptor(objectTrackingState),
                                        new PropertyChangedInterceptor(objectTrackingState),
                                        new ObjectInterceptor(objectTrackingState)
                                        );

        return result;
    }

    /// <summary>
    /// AsTrackable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T AsTrackable<T>(this T obj)
        where T : class
    {
        return (T)AsTrackable((object)obj);
    }

    /// <summary>
    /// AsTrackableCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IList<T> AsTrackableCollection<T>(this IList<T> obj)
        where T : class
    {
        if (obj is ITrackableCollection<T> trackableCollection)
        {
            return trackableCollection;
        }

        ObjectTrackingState objectTrackingState = new ObjectTrackingState(obj);

        ITrackableCollection<T> result = (ITrackableCollection<T>)Generator.CreateInterfaceProxyWithTarget(
                                    typeof(IList<T>),
                                    new[] { typeof(ITrackableCollection<T>), typeof(INotifyCollectionChanged) },
                                    obj,
                                    new CollectionInterceptor<T>(objectTrackingState, null)
                                    );

        return result;
    }

    /// <summary>
    /// AsTrackingCollection
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="objectTrackingState"></param>
    /// <returns></returns>
    internal static ITrackableCollection AsTrackingCollection(this object obj, string property, ObjectTrackingState objectTrackingState)
    {
        Type itemType = obj.GetType().GetGenericArguments()[0];

        ITrackableCollection result = (ITrackableCollection)
                                Generator.CreateInterfaceProxyWithTarget(
                                    typeof(IList<>).MakeGenericType(itemType),
                                    new[] { typeof(IList), typeof(ITrackableCollection), typeof(ITrackableCollection<>).MakeGenericType(itemType), typeof(INotifyCollectionChanged) },
                                    obj,
                                    (IInterceptor)Activator.CreateInstance(typeof(CollectionInterceptor<>).MakeGenericType(itemType), new object[] { objectTrackingState, property })
                                    );


        return result;
    }

    /// <summary>
    /// CastToTrackable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static ITrackableObject CastToTrackable<T>(this T obj)
        where T : class
    {
        return (ITrackableObject)obj;
    }

    /// <summary>
    /// CastToTrackable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static ITrackableCollection<T> CastToTrackableCollection<T>(this IList<T> obj)
        where T : class
    {
        return (ITrackableCollection<T>)obj;
    }
}
