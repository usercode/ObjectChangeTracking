using Core.Reflection;
using ObjectChangeTracking.Abstractions;
using ObjectChangeTracking.Changes;

namespace ObjectChangeTracking;

internal sealed class ObjectTrackingState
{
    private IDictionary<string, object?> _originalValues;
    private IDictionary<string, ITrackableCollection> _collectionProxies;
    
    public ObjectTrackingState(object target)
    {
        Target = target;
        _originalValues = new Dictionary<string, object?>();
        _collectionProxies = new Dictionary<string, ITrackableCollection>();
    }

    /// <summary>
    /// Target
    /// </summary>
    public object Target { get; }

    public void AddChangedProperty(string propertyName, object? oldValue)
    {
        //first change of property?
        if (_originalValues.ContainsKey(propertyName) == false)
        {
            _originalValues[propertyName] = oldValue;
        }
    }

    public ITrackableCollection? GetCollection(string property)
    {
        if (_collectionProxies.TryGetValue(property, out ITrackableCollection? value))
        {
            return value;
        }

        return null;
    }

    public void SetCollection(string property, ITrackableCollection list)
    {
        _collectionProxies[property] = list;
    }

    public IEnumerable<IPropertyChange> GetChangedProperties()
    {
        IEnumerable<IPropertyChange> simpleProperties = _originalValues.Select(x => new SimplePropertyChange(
                                                                x.Key,
                                                                x.Value,
                                                                PropertyAccessor.Get(Target.GetType().GetProperty(x.Key)).GetValue(Target)));

        IEnumerable<IPropertyChange> collectionProperties = _collectionProxies
                                                .Where(x => x.Value.Added.Any() || x.Value.Removed.Any())
                                                .Select(x => new CollectionPropertyChange(x.Key, x.Value.Added, x.Value.Removed));

        return simpleProperties.Concat(collectionProperties).ToList();
    }

    public bool AnyChanges()
    {
        return _originalValues.Any() || _collectionProxies.Any(x => x.Value.Added.Any() || x.Value.Removed.Any());
    }        
}
