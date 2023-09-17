namespace CSharpier.Cli.Options;

using System.Text.Json;
using System.Text.Json.Serialization;

internal class CaseInsensitiveEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct
{
    public override TEnum Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException(
                $"Expected a string for enum parsing, but got {reader.TokenType}."
            );
        }

        var enumText = reader.GetString();
        if (Enum.TryParse(enumText, ignoreCase: true, out TEnum result))
        {
            return result;
        }

        throw new JsonException($"Unable to parse '{enumText}' to enum of type {typeof(TEnum)}.");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var enumText = value.ToString();
        writer.WriteStringValue(enumText);
    }
}
