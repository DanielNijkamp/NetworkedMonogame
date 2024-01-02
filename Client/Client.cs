using System;
using System.Reflection;
using Commands.EntityCommands;
using ECS;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameNetworking.ECS.Components;
using MonoGameNetworking.ECS.System;
using MonoGameNetworking.ECS.System.Movement;

namespace MonoGameNetworking;

public class Client : Game
{
    //SignalR & MediatR object
    private readonly ClientNetworker clientNetworker;
    private readonly IServiceProvider serviceProvider;
    
    //Monogame declaration
    private static GraphicsDeviceManager _graphics;
    
    //ECS declaration
    private readonly BaseWorld baseWorld;
    private readonly InputSystem inputSystem;
    private readonly RenderSystem renderSystem;
    
    //private readonly TransformSystem transformSystem;
    private readonly NetworkedTransformSystem networkedTransformSystem;
    

    //Client data
    private readonly Guid ClientID;
    
    public Client()
    {
        //Client data initialization
        ClientID = Guid.NewGuid();
        
        serviceProvider = ConfigureServices();
        
        _graphics = InitGraphics();
        
        baseWorld = serviceProvider.GetRequiredService<BaseWorld>();
        renderSystem = serviceProvider.GetRequiredService<RenderSystem>();
        inputSystem = serviceProvider.GetRequiredService<InputSystem>();
        clientNetworker = serviceProvider.GetRequiredService<ClientNetworker>();
        networkedTransformSystem = serviceProvider.GetRequiredService<NetworkedTransformSystem>();
        
        if (AskForConnection())
        {
            CreatePlayer();
        }
        else
        {
            Environment.Exit(1);
        }
    }

    #region Monogame Methods
    protected override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();
        
        if (state.IsKeyDown(Keys.Escape))
            Exit();
        
        networkedTransformSystem.Process();
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        renderSystem.Process();
        base.Draw(gameTime);
    }

    /*protected override void LoadContent()
    {
        //Game logic initialization
        //baseWorld.CreateEntity(ClientID, new TransformComponent{Velocity = 5f}, new RenderComponent(_graphics.GraphicsDevice, Content.Load<Texture2D>("ball")));
        //transformSystem.Initialize(ClientID);
    }*/
    #endregion
    
    private bool AskForConnection()
    {
        Console.WriteLine("Do you want to connect? | Type [Y/N]");
        var result = Console.Read();
        return result is 'Y' or 'y';
    }

    private async void CreatePlayer()
    {
        await clientNetworker.SendCommand(new CreateEntityCommand 
        { 
            EntityID = ClientID, 
            Components = new IComponent[] 
            { 
                new TransformComponent{Velocity = 5f},
                new RenderComponent(_graphics.GraphicsDevice, Content.Load<Texture2D>("ball"))
            }
        });
    }
    
    private GraphicsDeviceManager InitGraphics()
    {
        //turn on to burn your GPU
        //_graphics.SynchronizeWithVerticalRetrace = false;
        //base.IsFixedTimeStep = false;
        
        var graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        return graphics;
    }
    
    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddSingleton<BaseWorld>();
        
        services.AddSingleton<InputSystem>();
        services.AddSingleton<RenderSystem>();
        services.AddSingleton<NetworkedTransformSystem>();
        
        //services.AddSingleton<TransformSystem>();
        
        return services.BuildServiceProvider();
    }
}