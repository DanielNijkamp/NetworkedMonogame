using System;
using ECS;
using ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameNetworking.ECS.System;

public class RenderSystem : ISystem
{
    private readonly World world;
    private SpriteBatch spriteBatch;
    private readonly SpriteStorage spriteStorage;
    
    private readonly GraphicsDeviceManager graphicsDeviceManager;
    public RenderSystem(World world, GraphicsDeviceManager graphicsDeviceManager, SpriteStorage spriteStorage)
    {
        this.world = world;
        this.graphicsDeviceManager = graphicsDeviceManager;
        this.spriteStorage = spriteStorage;
    }

    public void LoadContent()
    {
        spriteBatch = new SpriteBatch(graphicsDeviceManager.GraphicsDevice);
    }
    
    public void Process()
    {
        //var components = world.GetComponentForTypesImmutable<Component>(typeof(TransformComponent), typeof(RenderComponent));
        
        var transformComponents = world.GetComponentsOfTypeImmutable<TransformComponent>();
        var renderComponents = world.GetComponentsOfTypeImmutable<RenderComponent>();
        
        var length = transformComponents.Length;
        
        if (transformComponents.Length == 0 || renderComponents.Length == 0)
        {
            Console.WriteLine("No transform or render components found");
            return;
        }
          
        
        // Start rendering
       spriteBatch.Begin();
       
        for (int i = 0; i < length; i++)
        {
            var transformComponent = transformComponents[i];
            var renderComponent = renderComponents[i];

            if (transformComponent == null || renderComponent == null)
            {
                Console.WriteLine($"[{i}]nd/th Iteration failed to render due to null components");
                continue;
            }

            var texture = spriteStorage.GetSprite(renderComponent.TextureName);
            if (texture == null)
            {
                Console.WriteLine($"error with loading texture");
                continue;
            }
            
            spriteBatch.Draw(
                texture,
                transformComponent.Position,
                null,
                Color.White,
                0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }
        spriteBatch.End();
        
    }
}