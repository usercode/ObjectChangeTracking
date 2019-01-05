using Castle.DynamicProxy;
using ObjectChangeTracking.Abstractions;
using ObjectChangeTracking.Interceptors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace ObjectChangeTracking
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        internal static ProxyGenerator Generator = new ProxyGenerator();

        /// <summary>
        /// AsTrackable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T AsTrackable<T>(this T obj)
            where T : class
        {
            //is the object already a trackable object?
            if(obj is ITrackableObject)
            {
                return obj;
            }

            ObjectTrackingState objectTrackingState = new ObjectTrackingState(obj);

            if (obj is IEnumerable<Object>)
            {
                Type itemType = obj.GetType().GetGenericArguments()[0];
                T result = (T)Generator.CreateInterfaceProxyWithTarget(
                                        typeof(IList<T>),
                                        new[] { typeof(ITrackableCollection<T>), typeof(INotifyCollectionChanged) },
                                        obj,
                                        new CollectionInterceptor<T>(objectTrackingState, null)
                                        );

                return result;
            }
            else
            {
                T result = (T)Generator.CreateClassProxyWithTarget(
                                typeof(T),
                                new[] { typeof(ITrackableObject), typeof(INotifyPropertyChanging), typeof(INotifyPropertyChanged) },
                                obj,
                                new PropertyChangingInterceptor(objectTrackingState),
                                new PropertyChangedInterceptor(objectTrackingState),
                                new ObjectInterceptor(objectTrackingState)
                                );

                return result;
            }
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
            ObjectTrackingState objectTrackingState = new ObjectTrackingState(obj);

            IList<T> result = (IList<T>)Generator.CreateInterfaceProxyWithTarget(
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
        internal static Object AsTrackingCollection(this Object obj, String property, ObjectTrackingState objectTrackingState)
        {
            Type itemType = obj.GetType().GetGenericArguments()[0];

            Object result = Generator.CreateInterfaceProxyWithTarget(
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
}
