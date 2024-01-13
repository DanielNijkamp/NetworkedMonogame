using MessagePack;

namespace Commands;

[MessagePackObject(keyAsPropertyName: true)]
public class CommandWrapper
{
    public required string CommandType { get; set; }
    public required object Command { get; set; }
}