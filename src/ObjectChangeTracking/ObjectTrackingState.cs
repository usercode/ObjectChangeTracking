using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Core.Reflection;
using ObjectChangeTracking.Abstractions;
using ObjectChangeTracking.Changes;

namespace ObjectChangeTracking
{
    class ObjectTrackingState
    {
        private IDictionary<String, Object> _originalValues;
        private IDictionary<String, ITrackableCollection> _collectionProxies;
        
        public ObjectTrackingState(object target)
        {
            Target = target;
            _originalValues = new Dictionary<String, Object>();
            _collectionProxies = new Dictionary<String, ITrackableCollection>();
        }

        /// <summary>
        /// Target
        /// </summary>
        public object Target { get; }

        public void AddChangedProperty(String propertyName, object oldValue)
        {
            if (_originalValues.TryGetValue(propertyName, out object value) == false)
            {
                _originalValues.Add(propertyName, oldValue);
            }
            //else
            //{
            //    _originalValues[propertyName] = oldValue;
            //}
        }

        public ITrackableCollection GetCollection(String property)
        {
            if (_collectionProxies.TryGetValue(property, out ITrackableCollection value))
            {
                return value;
            }

            return null;
        }

        public void SetCollection(string property, ITrackableCollection list)
        {
            if (_collectionProxies.ContainsKey(property) == false)
            {
                _collectionProxies.Add(property, list);
            }
            else
            {
                _collectionProxies[property] = list;
            }
        }

        public IEnumerable<IPropertyChange> GetChangedProperties()
        {
            var simpleProperties = _originalValues.Select(x => new SimplePropertyChange(
                                                                    x.Key,
                                                                    x.Value,
                                                                    PropertyAccessor.Get(Target.GetType().GetProperty(x.Key)).GetValue(Target)))
                                                            .Cast<IPropertyChange>();

            var collectionProperties = _collectionProxies
                                                    .Select(x => new CollectionPropertyChange(x.Key, x.Value.Added, x.Value.Removed))
                                                    .Cast<IPropertyChange>();                                                    

            return simpleProperties.Concat(collectionProperties).ToList();
        }

        public bool AnyChanges()
        {
            return _originalValues.Any() || _collectionProxies.Any(x => x.Value.Added.Any() || x.Value.Removed.Any());
        }        
    }
}
