using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HRM.Extensions.JsonConverters
{
    public class TimespanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value=reader.GetString();
            return TimeSpan.TryParse(value, out var result) ? result : TimeSpan.Zero;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}