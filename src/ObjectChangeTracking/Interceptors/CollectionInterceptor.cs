using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;

namespace ObjectChangeTracking.Interceptors
{
    class CollectionInterceptor<T> : BaseInterceptor
    {
        private event NotifyCollectionChangedEventHandler CollectionChanged;

        private IList<T> Added;
        private IList<T> Removed;

        /// <summary>
        /// Property
        /// </summary>
        public string Property { get; }

        public CollectionInterceptor(ObjectTrackingState objectTrackingState, string property)
            : base(objectTrackingState)
        {
            Added = new List<T>();
            Removed = new List<T>();

            Property = property;
        }

        public override void Intercept(IInvocation invocation)
        {
            String methodName = invocation.Method.Name;

            if (methodName == "get_Added")
            {
                invocation.ReturnValue = new ReadOnlyCollection<T>(Added);
            }
            else if (methodName == "get_Removed")
            {
                invocation.ReturnValue = new ReadOnlyCollection<T>(Removed);
            }
            else if (methodName == "get_Item")
            {
                invocation.Proceed();

                invocation.ReturnValue = invocation.ReturnValue.AsTrackable();

                return;
            }
            else if (methodName == "add_CollectionChanged")
            {
                CollectionChanged += (NotifyCollectionChangedEventHandler)invocation.Arguments[0];
            }
            else if (methodName == "remove_CollectionChanged")
            {
                CollectionChanged -= (NotifyCollectionChangedEventHandler)invocation.Arguments[0];
            }
            else
            {
                invocation.Proceed();

                if (methodName == nameof(IList.Add))
                {
                    RaiseCollectionChangedAdd((T)invocation.Arguments[0]);
                }
                else if (methodName == nameof(IList.Remove))
                {
                    RaiseCollectionChangedRemove((T)invocation.Arguments[0]);
                }
                else if (methodName == nameof(IList.Clear))
                {
                    //RaiseCollectionChangedClear();
                }
            }
        }

        private void OnCollectionChanged()
        {
            if(Property != null)
            {
                //ObjectTrackingState.AddChangedProperty(Property, null);
            }
        }

        private void RaiseCollectionChangedAdd(T item)
        {
            if (Added.Contains(item) == false)
            {
                Added.Add(item);
            }

            if(CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] { item }));
            }

            OnCollectionChanged();
        }

        private void RaiseCollectionChangedRemove(T item)
        {
            if(Removed.Contains(item) == false)
            {
                Removed.Add(item);
            }

            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }));
            }

            OnCollectionChanged();
        }

        //private void RaiseCollectionChangedClear()
        //{
        //    if (CollectionChanged != null)
        //    {
        //        CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        //    }
        //}
    }
}
