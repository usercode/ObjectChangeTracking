using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Core.Reflection
{
    /// <summary>
    /// PropertyAccessor
    /// </summary>
    public abstract class PropertyAccessor
    {
        private static readonly Dictionary<PropertyInfo, PropertyAccessor> _cache
             = new Dictionary<PropertyInfo, PropertyAccessor>();
        
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static PropertyAccessor Get(PropertyInfo propertyInfo)
        {
            if (!_cache.TryGetValue(propertyInfo, out PropertyAccessor accessor))
            {
                accessor = (PropertyAccessor)Activator.CreateInstance(typeof(PropertyAccessor<,>)
                                .MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType),
                                propertyInfo);
                
                _cache.Add(propertyInfo, accessor);
            }

            return accessor;
        }

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Name = propertyInfo.Name;
        }

        /// <summary>
        /// PropertyInfo
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public abstract Object GetValue(Object target);

        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public abstract void SetValue(Object target, Object value);
    }
}
