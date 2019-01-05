using ObjectChangeTracking.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking
{
    public class ChangeTrackingScope
    {
        private IDictionary<object, ITrackableObject> _cache;

        public ChangeTrackingScope()
        {
            _cache = new Dictionary<object, ITrackableObject>();
        }


    }
}
