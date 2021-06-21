using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    public static class SystemTypeExtensions
    {
        public static bool IsDynamic(this Type type) => typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type);
        public static List<TPropertyType> GetPropertiesOfType<TPropertyType>(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(p => type.IsAssignableFrom(p.PropertyType))
                .Select(pi => (TPropertyType)pi.GetValue(null))
                .ToList();
        }
    }

}
