using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Geex.Shared._ShouldMigrateToLib.Abstractions
{
    public abstract class Enumeration<T>
    {
        public Enumeration(T value)
        {
            if (this)
            {
                
            }
        }
        public string Name { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator T(Enumeration<T> value)
        {
            return value.Value;
        }

        public static bool TryParse<TImplementation>(T value, out TImplementation result)
        {
            result = Enumeration<T>.GetAll<TImplementation>().FirstOrDefault(x => x.Equals(value));
            return !Equals(result, default(TImplementation));
        }

        public static TImplementation Parse<TImplementation>(T value)
        {
            var result = Enumeration<T>.GetAll<TImplementation>().First(x => x.Equals(value));
            return result;
        }

        public static IEnumerable<TImplemention> GetAll<TImplemention>()
        {
            var type = typeof(TImplemention);
            var typeInfo = type.GetTypeInfo();
            var fields = typeInfo.GetFields(BindingFlags.Public |
                                                      BindingFlags.Static |
                                                      BindingFlags.DeclaredOnly);
            foreach (var info in fields)
            {
                if (info.GetValue(default(TImplemention)) is TImplemention locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object? obj)
        {
            var otherValue = obj is Enumeration<T> enumeration ? enumeration : default;
            if (otherValue.Equals(default))
            {
                return false;
            }
            var typeMatches = GetType() == obj?.GetType();
            var valueMatches = Value?.Equals(otherValue.Value);
            return typeMatches && valueMatches == true;
        }
        // Other utility methods ...
    }
}
