using ECS;
using MessagePack;

namespace Commands;

[MessagePackObject(keyAsPropertyName:true)]
public class CopyGameStateCommand : Command
{
    public EntityStorage Storage { get; set; }
}