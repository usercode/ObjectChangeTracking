using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ObjectChangeTracking.Reflection
{
    class PropertyMap
    {
        public PropertyMap(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo { get; }
    }
}
