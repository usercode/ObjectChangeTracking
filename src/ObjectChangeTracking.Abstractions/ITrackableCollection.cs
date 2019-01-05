using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking.Abstractions
{
    public interface ITrackableCollection : IEnumerable
    {
        IEnumerable<object> Added { get; }
        IEnumerable<object> Removed { get; }
    }
}
