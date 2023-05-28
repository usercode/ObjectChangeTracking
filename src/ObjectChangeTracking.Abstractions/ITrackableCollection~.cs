namespace ObjectChangeTracking.Abstractions;

public interface ITrackableCollection<T> : IList<T>
{
    IEnumerable<T> Added { get; }
    IEnumerable<T> Removed { get; }
}
