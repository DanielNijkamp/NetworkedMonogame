using MessagePack;

namespace Commands.EntityCommands;

[MessagePackObject(keyAsPropertyName: true)]
public class DeleteEntityCommand : Command
{
    
}