using MessagePack;
using MessagePack.Formatters;
using Microsoft.Xna.Framework;

namespace Serialization;

public class Vector2Formatter : IMessagePackFormatter<Vector2>
{
    public void Serialize(ref MessagePackWriter writer, Vector2 value, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(2);  // Vector2 has two properties: X and Y
        writer.Write(value.X);
        writer.Write(value.Y);
    }

    public Vector2 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.TryReadNil())
        {
            throw new MessagePackSerializationException("Deserialized value cannot be null.");
        }

        var length = reader.ReadArrayHeader();
        if (length != 2)  // Vector2 has two properties: X and Y
        {
            throw new MessagePackSerializationException("Invalid Vector2 format");
        }

        var X = reader.ReadSingle(); // Reading as a single precision floating point number
        var Y = reader.ReadSingle(); // Reading as a single precision floating point number

        return new Vector2(X, Y);
    }
}