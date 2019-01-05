using ObjectChangeTracking.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking.Changes
{
    /// <summary>
    /// CollectionPropertyChange
    /// </summary>
    public class CollectionPropertyChange : IPropertyChange
    {
        public CollectionPropertyChange(string name, IEnumerable<Object> added, IEnumerable<Object> removed)
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
}
