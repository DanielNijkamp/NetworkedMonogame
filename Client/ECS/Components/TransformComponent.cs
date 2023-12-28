using ECS;
using Microsoft.Xna.Framework;

namespace MonoGameNetworking.ECS.Components;

public class TransformComponent : IComponent
{
    public Vector2 Position { get; set; } = new (0, 0);
    public float Velocity { get; set; }
}