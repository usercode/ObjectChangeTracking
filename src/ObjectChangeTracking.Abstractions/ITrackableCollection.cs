using System.Collections;

namespace ObjectChangeTracking.Abstractions;

public interface ITrackableCollection : IEnumerable
{
    IEnumerable<object> Added { get; }
    IEnumerable<object> Removed { get; }
}
