using ObjectChangeTracking.Abstractions;

namespace ObjectChangeTracking.Changes;

/// <summary>
/// CollectionPropertyChange
/// </summary>
public sealed class CollectionPropertyChange : IPropertyChange
{
    public CollectionPropertyChange(string name, IEnumerable<object> added, IEnumerable<object> removed)
    {
        Name = name;
        Added = added;
        Removed = removed;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Added
    /// </summary>
    public IEnumerable<object> Added { get; }

    /// <summary>
    /// Removed
    /// </summary>
    public IEnumerable<object> Removed { get; }
}
