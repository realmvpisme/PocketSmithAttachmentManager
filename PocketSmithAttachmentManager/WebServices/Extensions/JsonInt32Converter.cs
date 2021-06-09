using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PocketSmithAttachmentManager.WebServices.Extensions
{
    public class JsonInt32Converter : System.Text.Json.Serialization.JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = Regex.Match(reader.GetString(), "\\d+").Value;

                if (int.TryParse(stringValue, out int value))
                {
                    return value;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}