using System.Linq.Expressions;
using System.Reflection;

namespace ShoeStore.Application.Utilities;

public static class SortExpression
{
    public static Expression<Func<T, object>> BuildOrDefault<T>(string? propertyName, Expression<Func<T, object>> defaultExpression)
    {
        return Build<T>(propertyName) ?? defaultExpression;
    }

    public static Expression<Func<T, object>>? Build<T>(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return null;
        }

        var type = typeof(T);
        var property = type.GetProperty(
            propertyName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property is null)
        {
            return null;
        }

        var param = Expression.Parameter(type, "x");
        var propertyAccess = Expression.Property(param, property);
        var converted = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<T, object>>(converted, param);
    }
}