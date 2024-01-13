using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameNetworking;

public class SpriteStorage
{
    private readonly ContentManager content;
    private readonly Dictionary<string, Texture2D> spriteMap = new ();

    public SpriteStorage(ContentManager content)
    {
        this.content = content;
    }
    
    public void CreateMapping(string textureName)
    {
        var texture = content.Load<Texture2D>(textureName);
        spriteMap.TryAdd(textureName, texture);
    }
    
    public Texture2D? GetSprite(string key)
    {
        return spriteMap.GetValueOrDefault(key);
    }
    
}