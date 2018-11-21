using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ObjectChangeTracking.Metadata;
using System.Reflection;

namespace ObjectChangeTracking
{
    class ObjectTrackingState
    {
        private IDictionary<String, Object> _originalValues;
        private IDictionary<String, Object> _collectionProxies;
        
        public ObjectTrackingState()
        {
            _originalValues = new Dictionary<String, Object>();
            _collectionProxies = new Dictionary<String, Object>();
        }

        public void AddChangedProperty(String propertyName, object newValue)
        {
            if (_originalValues.TryGetValue(propertyName, out object value) == false)
            {
                _originalValues.Add(propertyName, null);
            }
            else
            {
                _originalValues[propertyName] = newValue;
            }
        }

        public Object GetCollection(String property)
        {
            if (_collectionProxies.TryGetValue(property, out Object value))
            {
                return value;
            }

            return null;
        }

        public void SetCollection(String property, Object list)
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

        public IEnumerable<ChangedProperty> GetChangedProperties()
        {
            return _originalValues.Keys.Select(x => new ChangedProperty(x, null));
        }

        public bool AnyChanges()
        {
            return _originalValues.Any() || _collectionProxies.Any();
        }        
    }
}
