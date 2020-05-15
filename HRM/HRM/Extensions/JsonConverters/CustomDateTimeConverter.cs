using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HRM.Extensions.JsonConverters
{
    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value=reader.GetString();
            if (value == null || value.Equals(""))
            {
                return null;
            }
            return DateTime.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.Value);
            }
        }
    }
}