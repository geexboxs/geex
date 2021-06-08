using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geex.Shared._ShouldMigrateToLib
{
    public static class SystemTypeExtensions
    {
        public static object Default(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static bool IsDynamic(this Type type) => typeof (IDynamicMetaObjectProvider).IsAssignableFrom(type);
    }
}
