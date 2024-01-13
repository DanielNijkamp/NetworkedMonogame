using MessagePack;
using Microsoft.Xna.Framework;

namespace Commands.EntityCommands;

[MessagePackObject(keyAsPropertyName:true)]
public class MovementCommand : Command
{
    public required Guid EntityID { get; set; }
    public Vector2 MovementVector { get;  set; }
}