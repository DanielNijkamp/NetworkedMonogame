using MessagePack;
using Microsoft.Xna.Framework;

namespace Commands.EntityCommands;

[MessagePackObject(keyAsPropertyName:true)]
public class MovementCommand : Command
{
    public Vector2 MovementVector { get; private set; }
    public MovementCommand(Guid entityId, Vector2 vector2)
    {
        EntityID = entityId;
        MovementVector = vector2;
    }
}