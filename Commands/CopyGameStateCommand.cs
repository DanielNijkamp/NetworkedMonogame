using ECS;

namespace Commands;

public class CopyGameStateCommand : Command
{
    public EntityStorage Storage { get; set; }
}