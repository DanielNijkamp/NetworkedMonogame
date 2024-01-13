using ECS;
using MessagePack;

namespace Commands.EntityCommands;

[MessagePackObject(keyAsPropertyName: true)]
public class CreateEntityCommand : Command
{
    public required Guid EntityID { get; set; }
    public required Component[] Components { get; set; }
}