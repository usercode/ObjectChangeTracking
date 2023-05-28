using System.Reflection;

namespace ObjectChangeTracking;

static class ReflectionExtensions
{
    public static bool IsGetterMethod(this MethodInfo methodInfo, string? propertyName = null)
    {
        if (propertyName == null)
        {
            return methodInfo.IsSpecialName && methodInfo.Name.StartsWith(ReflectionHelper.GetterPrefix);
        }
        else
        {
            return methodInfo.IsSpecialName && methodInfo.Name == ReflectionHelper.GetterPrefix + propertyName;
        }
    }

    public static bool IsSetterMethod(this MethodInfo methodInfo, string? propertyName = null)
    {
        if (propertyName == null)
        {
            return methodInfo.IsSpecialName && methodInfo.Name.StartsWith(ReflectionHelper.SetterPrefix);
        }
        else
        {
            return methodInfo.IsSpecialName && methodInfo.Name == ReflectionHelper.SetterPrefix + propertyName;
        }
    }

    public static string GetPropertyName(this MethodInfo methodInfo)
    {
        return methodInfo.Name.Substring(ReflectionHelper.GetterPrefix.Length);
    }

    public static bool IsCollection(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);
    }
}
