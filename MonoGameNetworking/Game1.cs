using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameNetworking.ECS;
using MonoGameNetworking.ECS.Components;
using MonoGameNetworking.ECS.System;

namespace MonoGameNetworking;

public class Game1 : Game
{
    private static GraphicsDeviceManager _graphics;
    
    private readonly EntityManager entityManager;

    private readonly InputSystem inputSystem;
    private readonly RenderSystem renderSystem;
    private readonly TransformSystem transformSystem;

    private readonly Guid ClientID;
    
    public Game1()
    {
        ClientID = Guid.NewGuid();
        //Monogame initialization
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        //Infrastructure initialization
        entityManager = new EntityManager();
        renderSystem = new RenderSystem(entityManager);
        inputSystem = new InputSystem(ClientID);
        transformSystem = new TransformSystem(entityManager, inputSystem);
        
    }
    protected override void LoadContent()
    {
        //Game logic initialization
        var player = entityManager.CreateEntity(ClientID);
        player.AddComponent(new TransformComponent{Velocity = 5f});
        player.AddComponent(new RenderComponent(_graphics.GraphicsDevice, Content.Load<Texture2D>("ball")));
        
        transformSystem.Initialize(ClientID);
    }
    
    protected override void Update(GameTime gameTime)
    {
        transformSystem.Process();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        renderSystem.Process();
        base.Draw(gameTime);
        
    }
}