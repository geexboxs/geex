using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Geex.Shared._ShouldMigrateToLib.Json.Converters
{
    public class JsonEnumConverter : JsonConverterFactory
    {
        private readonly JsonStringEnumConverter _innerJsonStringEnumConverter = new JsonStringEnumConverter();

        private class JsonFlagEnumConverterInstance<T> : JsonConverter<T>
        {
            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return (T)(object)reader.GetInt32();
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue((int)(object)value);
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert.GetCustomAttribute(typeof(FlagsAttribute)) == null)
            {
                return this._innerJsonStringEnumConverter.CreateConverter(typeToConvert,options);
            }
            return Activator.CreateInstance(typeof(JsonFlagEnumConverterInstance<>).MakeGenericType(typeToConvert)) as JsonConverter;
        }
    }
}
