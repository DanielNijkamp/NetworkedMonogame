using System;
using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameNetworking.ECS.Components;
using MonoGameNetworking.ECS.System;
using MonoGameNetworking.ECS.System.Movement;

namespace MonoGameNetworking;

public class Client : Game
{
    
    //SignalR decleration
    //private readonly HubConnection hubConnection;
    
    //Monogame declaration
    private static GraphicsDeviceManager _graphics;
    
    //ECS declaration
    private readonly World world;
    private readonly InputSystem inputSystem;
    private readonly RenderSystem renderSystem;
    private readonly TransformSystem transformSystem;

    //Client data
    private readonly Guid ClientID;
    
    public Client()
    {
        //Client data initialization
        ClientID = Guid.NewGuid();
        
        //signalR initialization
        /*hubConnection = new HubConnectionBuilder()
            .WithUrl("")
            .Build();

        hubConnection.StartAsync();*/
        
        //Monogame initialization
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        //Infrastructure initialization
        world = new World();
        renderSystem = new RenderSystem(world);
        inputSystem = new InputSystem(ClientID);
        transformSystem = new TransformSystem(world, inputSystem);
        
        //turn on to burn your GPU
        //_graphics.SynchronizeWithVerticalRetrace = false;
        //base.IsFixedTimeStep = false;
        
    }
    protected override void LoadContent()
    {
        //Game logic initialization
        world.CreateEntity(ClientID, new TransformComponent{Velocity = 5f}, new RenderComponent(_graphics.GraphicsDevice, Content.Load<Texture2D>("ball")));
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