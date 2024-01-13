using MessagePack;
using Microsoft.Xna.Framework;

namespace ECS.Components;

[MessagePackObject(keyAsPropertyName:true)]
public class TransformComponent : Component
{
    public Vector2 Position { get; set; } = new (0, 0);
    public float Velocity { get; set; }
}