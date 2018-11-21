using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking.Abstractions
{
    public interface ITrackableCollection<out T> : ITrackableCollection, IEnumerable<T>
    {
        IEnumerable<T> Added { get; }
        IEnumerable<T> Removed { get; }
    }
}
