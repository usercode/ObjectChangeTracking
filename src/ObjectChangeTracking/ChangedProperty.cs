using ObjectChangeTracking.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking
{
    /// <summary>
    /// ChangedProperty
    /// </summary>
    public class ChangedProperty : IChangedProperty
    {
        public ChangedProperty(String name, Object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Value
        /// </summary>
        public Object Value { get; }
    }
}
