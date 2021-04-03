using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Geex.Shared._ShouldMigrateToLib.Json
{
    public static class Json
    {
        public static JsonSerializerSettings DefaultSerializeSettings { get; set; } = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            Converters = new List<JsonConverter>()
            {
                new StringEnumConverter()
            },
        };

        public static JsonSerializerSettings IgnoreErrorSerializeSettings { get; set; } = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Error = (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            },
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            Converters = new List<JsonConverter>()
            {
                new StringEnumConverter()
            },
        };

        public static string ToJson(this object @this, bool ignoreError = true)
        {
            if (ignoreError)
            {
                return JsonConvert.SerializeObject(@this, IgnoreErrorSerializeSettings);
            }
            else
            {
                return JsonConvert.SerializeObject(@this, DefaultSerializeSettings);
            }
        }

        public static T ToObject<T>(this string @this, bool ignoreError = true)
        {
            if (ignoreError)
            {
                return JsonConvert.DeserializeObject<T>(@this, IgnoreErrorSerializeSettings);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(@this, DefaultSerializeSettings);
            }
        }
    }
}
