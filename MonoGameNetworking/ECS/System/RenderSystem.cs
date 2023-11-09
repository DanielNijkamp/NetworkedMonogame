using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameNetworking.ECS.Components;

namespace MonoGameNetworking.ECS.System;

public class RenderSystem : ISystem
{
    private readonly EntityManager entityManager;

    public RenderSystem(EntityManager entityManager)
    {
        this.entityManager = entityManager;
    }
    
    public void Process()
    {
        foreach (var entity in entityManager.GetEntitiesWith(typeof(TransformComponent), typeof(RenderComponent)))
        {
            var p = entity.GetComponent<TransformComponent>();
            var c = entity.GetComponent<RenderComponent>();
            
            c.Sprite.Begin();
            c.Sprite.Draw(
                c.Texture,
                p.Position,
                null,
                Color.White,
                0f,
                new Vector2(c.Texture.Width / 2, c.Texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            c.Sprite.End();
        }
        
    }
}