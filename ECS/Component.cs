using ECS.Components;
using MessagePack;

namespace ECS;

[Union(key:0, typeof(RenderComponent))]
[Union(key:1, typeof(TransformComponent))]
public abstract class Component
{
    
}