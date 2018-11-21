using Castle.DynamicProxy;
using System;
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

        public String Property { get; private set; }

        public CollectionInterceptor(ObjectTrackingState objectTrackingState, String property)
            : base(objectTrackingState)
        {
            Added = new List<T>();
            Removed = new List<T>();

            Property = property;
        }


        public override void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "get_Added")
            {
                invocation.ReturnValue = Added;

                return;
            }
            else if (invocation.Method.Name == "get_Removed")
            {
                invocation.ReturnValue = Removed;

                return;
            }

            if (invocation.Method.Name == "add_CollectionChanged")
            {
                CollectionChanged += (NotifyCollectionChangedEventHandler)invocation.Arguments[0];
            }
            else if (invocation.Method.Name == "remove_CollectionChanged")
            {
                CollectionChanged -= (NotifyCollectionChangedEventHandler)invocation.Arguments[0];
            }
            else
            {
                invocation.Proceed();

                if (invocation.Method.Name == nameof(IList.Add))
                {
                    RaiseCollectionChangedAdd((T)invocation.Arguments[0]);
                }
                else if (invocation.Method.Name == nameof(IList.Remove))
                {
                    RaiseCollectionChangedRemove((T)invocation.Arguments[0]);
                }
                else if (invocation.Method.Name == nameof(IList.Clear))
                {
                    //RaiseCollectionChangedClear();
                }

            }
        }

        private void OnCollectionChanged()
        {
            if(Property != null)
            {
                ObjectTrackingState.AddChangedProperty(Property, null);
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
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
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
