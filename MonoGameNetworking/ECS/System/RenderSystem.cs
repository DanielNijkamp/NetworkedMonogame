﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameNetworking.ECS.Components;

namespace MonoGameNetworking.ECS.System;

public class RenderSystem : ISystem
{
    private readonly World _world;

    public RenderSystem(World world)
    {
        this._world = world;
    }
    
    public void Process()
    {
        var components = _world.GetComponentForTypesImmutable(typeof(TransformComponent), typeof(RenderComponent));

        var length = components[0].Count;
        for (int i = 0; i < length; i++)
        {
            var transformComponent = components[0][i] as TransformComponent;
            var renderComponent = components[1][i] as RenderComponent;
            
            renderComponent.Sprite.Begin();
            renderComponent.Sprite.Draw(
                renderComponent.Texture,
                transformComponent.Position,
                null,
                Color.White,
                0f,
                new Vector2(renderComponent.Texture.Width / 2, renderComponent.Texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            renderComponent.Sprite.End();
        }
        
    }
}