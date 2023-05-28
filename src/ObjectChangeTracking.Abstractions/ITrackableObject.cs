namespace ObjectChangeTracking.Abstractions;

/// <summary>
/// ITrackableObject
/// </summary>
public interface ITrackableObject
{
    /// <summary>
    /// IsChanged
    /// </summary>
    bool IsChanged { get; }

    /// <summary>
    /// ChangedProperties
    /// </summary>
    IEnumerable<IPropertyChange> ChangedProperties { get; }
}
