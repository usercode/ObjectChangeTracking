using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking.Abstractions
{
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
        IEnumerable<IChangedProperty> ChangedProperties { get; }
    }
}
