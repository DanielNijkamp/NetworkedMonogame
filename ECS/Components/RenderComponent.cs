using MessagePack;
using Microsoft.Xna.Framework.Graphics;

namespace ECS.Components;

[MessagePackObject(keyAsPropertyName:true)]
public class RenderComponent : Component
{
    public required string TextureName { get; set; }
}