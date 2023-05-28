using System.Reflection;

namespace Core.Reflection;

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
    public static PropertyAccessor Get(PropertyInfo? propertyInfo)
    {
        if (propertyInfo == null)
        {
            throw new ArgumentNullException(nameof(propertyInfo));
        }

        if (!_cache.TryGetValue(propertyInfo, out PropertyAccessor? accessor))
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
    public string Name { get; private set; }

    /// <summary>
    /// GetValue
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public abstract object? GetValue(object target);

    /// <summary>
    /// SetValue
    /// </summary>
    /// <param name="target"></param>
    /// <param name="value"></param>
    public abstract void SetValue(object target, object? value);
}
