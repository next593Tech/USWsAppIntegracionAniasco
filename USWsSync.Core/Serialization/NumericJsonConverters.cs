using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace USWsSync.Core.Serialization
{
    public class CustomDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }
            if (reader.TokenType == JsonTokenType.String && decimal.TryParse(reader.GetString(), out var d))
            {
                return d;
            }
            return 0m;
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            if (value % 1 == 0 && value >= long.MinValue && value <= long.MaxValue)
            {
                writer.WriteNumberValue((long)value);
            }
            else
            {
                var normalized = value / 1.000000000000000000000000000000000m;
                writer.WriteNumberValue(normalized);
            }
        }
    }

    public class NullableDecimalConverter : JsonConverter<decimal?>
    {
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDecimal();
            }
            if (reader.TokenType == JsonTokenType.String && decimal.TryParse(reader.GetString(), out var d))
            {
                return d;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }

            var val = value.Value;
            if (val % 1 == 0 && val >= long.MinValue && val <= long.MaxValue)
            {
                writer.WriteNumberValue((long)val);
            }
            else
            {
                var normalized = val / 1.000000000000000000000000000000000m;
                writer.WriteNumberValue(normalized);
            }
        }
    }

    public class CustomDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.GetDouble();

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (value % 1 == 0 && value >= long.MinValue && value <= long.MaxValue)
            {
                writer.WriteNumberValue((long)value);
            }
            else
            {
                writer.WriteNumberValue(value);
            }
        }
    }

    public class NullableDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.TokenType == JsonTokenType.Null ? null : reader.GetDouble();

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }
            var val = value.Value;
            if (val % 1 == 0 && val >= long.MinValue && val <= long.MaxValue)
            {
                writer.WriteNumberValue((long)val);
            }
            else
            {
                writer.WriteNumberValue(val);
            }
        }
    }
}