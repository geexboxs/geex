﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using HotChocolate.Types.Descriptors;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class GeexTypeConvention : DefaultTypeInspector
    {
        protected override void Initialize(IConventionContext context)
        {
            base.Initialize(context);
        }

        public override IEnumerable<MemberInfo> GetMembers(Type type)
        {
            return base.GetMembers(type);
        }

        public override IEnumerable<object> GetEnumValues(Type enumType)
        {
            if ((object)enumType == null)
                throw new ArgumentNullException(nameof(enumType));

            if (enumType != typeof(object) && enumType.IsEnum)
            {
                return Enum.GetValues(enumType).Cast<object>();
            }

            if (enumType.IsAssignableTo<IEnumeration>())
            {
                var genericImplementation = enumType.GetBaseClasses().FirstOrDefault(x => x.Name == (typeof(Enumeration<,>).Name));
                return ((System.Collections.IEnumerable)genericImplementation?.GetProperty(nameof(Enumeration.List))?.GetValue(null)).Cast<object>();
            }
            return Enumerable.Empty<object>();
        }

        protected override void Complete(IConventionContext context)
        {
            base.Complete(context);
        }
    }
}
