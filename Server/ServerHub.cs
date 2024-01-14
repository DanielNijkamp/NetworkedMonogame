using System.Collections.Concurrent;
using Commands;
using Commands.EntityCommands;
using ECS;
using MediatR;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Server;

public class ServerHub : Hub
{
    private static ConcurrentDictionary<string, Guid> connectedClients = new();
    
    private readonly IMediator mediator;
    private readonly World world;
    public ServerHub(IMediator mediator, World world)
    {
        this.mediator = mediator;
        this.world = world;
        Console.WriteLine("ServerHub is instantiated");
    }
    
    public async Task ReceiveCommandWrapper(CommandWrapper wrapper)
    {
        Console.WriteLine($"Received [{wrapper.Command}]");
        var type = Type.GetType(wrapper.CommandType);
        var command = MessagePackSerializer.Deserialize(type, MessagePackSerializer.Serialize(wrapper.Command)) as IRequest;
        await mediator.Send(command);
        await BroadcastCommand(wrapper);
    }
    
    public override async Task OnConnectedAsync()
    {
        await CopyGameState(world);
        Console.WriteLine($"Initializing client");
        await Clients.Client(Context.ConnectionId).SendAsync("RequestInit");
        await base.OnConnectedAsync();
        Console.WriteLine($"Client initialized");
    }

    public async Task CopyGameState(World world)
    {   
        Console.WriteLine("Copying game state to client");
        await Clients.Caller.SendAsync("ReceiveCommandWrapper", new CommandWrapper
        {
            CommandType = typeof(CopyGameStateCommand).AssemblyQualifiedName, 
            Command = new CopyGameStateCommand { Storage = world.storage}
        
        });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var disconnectedPlayer = connectedClients[Context.ConnectionId];
        await Clients.Others.SendAsync("ReceiveCommandWrapper", new CommandWrapper
        {
            CommandType = typeof(DeleteEntityCommand).AssemblyQualifiedName,
            Command = new DeleteEntityCommand{EntityID = disconnectedPlayer}
        });
        await base.OnDisconnectedAsync(exception);
    }
    public async Task InitClient(Guid entityId)
    {
        connectedClients.TryAdd(Context.ConnectionId, entityId);
        Console.WriteLine("Mapped PlayerID to connectionID");
        await Clients.Client(Context.ConnectionId).SendAsync("CreatePlayer");
    }

    public async Task BroadcastCommand(CommandWrapper wrapper)
    {
        Console.WriteLine($"Broadcasting: [{wrapper}]");
        await Clients.All.SendAsync("ReceiveCommandWrapper", wrapper);
    }
}