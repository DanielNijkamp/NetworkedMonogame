using System;
using System.Reflection;
using ECS;
using MediatR;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameNetworking.ECS.System;
using Serialization;

namespace MonoGameNetworking;

public class Client : Game
{
    //SignalR & MediatR object
    private readonly ClientNetworker clientNetworker;

    //Other essentials
    private readonly SpriteStorage spriteStorage;
    private readonly GraphicsDeviceManager graphicsDeviceManager;
    
    //ECS declaration
    private readonly World world;
    private readonly InputSystem inputSystem;
    private readonly RenderSystem renderSystem;
    
    //private readonly TransformSystem transformSystem;
    private readonly NetworkedTransformSystem networkedTransformSystem;
    
    //Client data
    private Guid clientId;
    private Color color;
    
    public Client()
    {
        var rnd = new Random();
        color =  new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        
        var serviceProvider = ConfigureServices();
        
        graphicsDeviceManager = serviceProvider.GetRequiredService<GraphicsDeviceManager>();
        spriteStorage = serviceProvider.GetRequiredService<SpriteStorage>();
        
        InitGraphics();
        
        world = serviceProvider.GetRequiredService<World>();
        
        renderSystem = serviceProvider.GetRequiredService<RenderSystem>();
        inputSystem = serviceProvider.GetRequiredService<InputSystem>();
        
        clientNetworker = serviceProvider.GetRequiredService<ClientNetworker>();
        networkedTransformSystem = serviceProvider.GetRequiredService<NetworkedTransformSystem>();
        
        EstablishConnection();
        
    }

    #region Monogame Methods
    protected override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Escape))
        {
            Console.WriteLine("Disconnecting from server...");
            clientNetworker.DisconnectAsync().Wait();
            Console.WriteLine("Disconnected from server");
        }
        networkedTransformSystem.Process();
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        graphicsDeviceManager.GraphicsDevice.Clear(color);
        renderSystem.Process();
        base.Draw(gameTime);
    }

    protected override void LoadContent()
    {
        spriteStorage.CreateMapping("ball");
        renderSystem.LoadContent();
        //Game logic initialization
        //baseWorld.CreateEntity(ClientID, new TransformComponent{Velocity = 5f}, new RenderComponent(_graphics.GraphicsDevice, Content.Load<Texture2D>("ball")));
        //transformSystem.Initialize(ClientID);
    }
    #endregion
    
    private async void EstablishConnection()
    {
        Console.WriteLine("Establish connection?");
        var input = Console.ReadLine()!;
        if (!input.Contains('Y') && !input.Contains('y')) return;
        Console.WriteLine(input);
        await clientNetworker.StartConnection();
    }
    
    private void InitGraphics()
    {
        //turn on to burn your GPU
        //graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
        //base.IsFixedTimeStep = false;
        //TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); //30?
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    
    private IServiceProvider ConfigureServices()
    {
        clientId = Guid.NewGuid();
        
        var services = new ServiceCollection();
        services.AddMediatR(cfg => 
        { 
            cfg.RegisterServicesFromAssembly(Assembly.Load("Handlers")); 
        });
        
        services.AddSingleton<GraphicsDeviceManager>(g => new GraphicsDeviceManager(this));
        
        services.AddSingleton<World>();
        services.AddSingleton<ClientNetworker>(serviceProvider => 
            new ClientNetworker(serviceProvider.GetRequiredService<IMediator>(), "http://localhost:5000/server", clientId));
        services.AddSingleton<InputSystem>(i => 
            new InputSystem(clientId));
        services.AddSingleton<SpriteStorage>(s => new SpriteStorage(Content));
        services.AddSingleton<RenderSystem>();
        services.AddSingleton<NetworkedTransformSystem>();
        
        //serialization
        var options = MessagePackSerializerOptions.Standard.
            WithResolver(
                CompositeResolver.Create(
                    new Vector2Formatter()) 
            )
            .WithResolver(
                ContractlessStandardResolver.Instance);
        
        //services.AddSingleton<TransformSystem>();
        
        return services.BuildServiceProvider();
    }
}