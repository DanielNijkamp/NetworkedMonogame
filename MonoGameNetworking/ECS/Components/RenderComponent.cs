using Microsoft.Xna.Framework.Graphics;

namespace MonoGameNetworking.ECS.Components;

public class RenderComponent : IComponent
{
    public RenderComponent(GraphicsDevice graphicsDevice, Texture2D texture)
    {
        Sprite = new SpriteBatch(graphicsDevice);
        Texture = texture;
    }
    public SpriteBatch Sprite { get; set; }
    public Texture2D Texture { get; private set; }
}