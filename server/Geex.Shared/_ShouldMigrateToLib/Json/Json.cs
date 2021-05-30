using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Unicode;
using Geex.Shared._ShouldMigrateToLib.Abstractions;
using Geex.Shared._ShouldMigrateToLib.Json.Converters;

namespace Geex.Shared._ShouldMigrateToLib.Json
{
    public static class Json
    {
        static Json()
        {
            var encoderSettings = new TextEncoderSettings();
            encoderSettings.AllowRange(UnicodeRanges.All);
            DefaultSerializeSettings.Encoder = JavaScriptEncoder.Create(encoderSettings);
            DefaultSerializeSettings.Converters.Add(new JsonStringEnumConverter());
            DefaultSerializeSettings.Converters.Add(new EnumerationConverterFactory());
            DefaultSerializeSettings.Converters.Add(new DynamicJsonConverter());
        }
        public static JsonSerializerOptions DefaultSerializeSettings { get; set; } = new();

        public static string ToJson(this object @this, bool ignoreError = true)
        {
            try
            {
                return JsonSerializer.Serialize(@this, DefaultSerializeSettings);
            }
            catch (Exception e)
            {
                if (ignoreError)
                {
                    throw;
                }
                return e.Message;
            }
        }

        public static T? ToObject<T>(this string @this, bool ignoreError = true)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(@this, DefaultSerializeSettings);
            }
            catch (Exception e)
            {
                if (ignoreError)
                {
                    throw;
                }
                return default;
            }
        }
    }

    public class EnumerationConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo<IEnumeration>();
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type enumType = typeToConvert.GetClassEnumRealType();

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(EnumerationConverter<>)
                    .MakeGenericType(new Type[] { enumType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;

            return converter;
        }
    }

    public class EnumerationConverter<T> : JsonConverter<T> where T : class
    {
        private static Type classEnumRealType = typeof(T).GetClassEnumRealType();
        private static Type classEnumValueType = typeof(T).GetClassEnumValueType();
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var data = reader.GetString();
            return typeof(Enumeration<,>).MakeGenericType(classEnumRealType, classEnumValueType).GetMethod(nameof(Enumeration.FromValue), types: new[] { classEnumValueType })?.Invoke(null, new[] { data }) as T;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var data = value.ToString();
            writer.WriteStringValue(data);
        }
    }
}
