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
        
        //Monogame initialization
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        //Infrastructure initialization
        baseWorld = new BaseWorld();
        renderSystem = new RenderSystem(baseWorld);
        inputSystem = new InputSystem(ClientID);
        //transformSystem = new TransformSystem(baseWorld, inputSystem);
        
        //networked initialization
        clientNetworker = new ClientNetworker(serviceProvider.GetRequiredService<IMediator>(), "http://127.0.0.1:8080");
        networkedTransformSystem = new NetworkedTransformSystem(baseWorld, inputSystem, clientNetworker);

        Console.WriteLine("Do you want to connect? | Type [Y/N]");
        var result = Console.Read();
        if (result is 'Y' or 'y')
        {
            //create our player
            clientNetworker.SendCommand(new CreateEntityCommand 
            { 
                EntityID = ClientID, 
                Components = new IComponent[] 
                    { 
                        new TransformComponent{Velocity = 5f} ,
                        new RenderComponent(_graphics.GraphicsDevice, Content.Load<Texture2D>("ball"))
                    }
            });
        }
        else
        {
            Environment.Exit(1);
        }

        //turn on to burn your GPU
        //_graphics.SynchronizeWithVerticalRetrace = false;
        //base.IsFixedTimeStep = false;



        //on startup: ask if user wants to connect
        //Client > Server connect > Client confirmation
        //Client asks server to create player with its client ID
        // Player can now use InputSystem with its own clientID to tell its own player to move

    }
    
    /*protected override void LoadContent()
    {
        //Game logic initialization
        //baseWorld.CreateEntity(ClientID, new TransformComponent{Velocity = 5f}, new RenderComponent(_graphics.GraphicsDevice, Content.Load<Texture2D>("ball")));
        //transformSystem.Initialize(ClientID);
    }*/
    
    protected override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();
        
        if (state.IsKeyDown(Keys.Escape))
            Exit();
        
        //transformSystem.Process();
        networkedTransformSystem.Process();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        renderSystem.Process();
        base.Draw(gameTime);
    }
    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        return services.BuildServiceProvider();
    }
}